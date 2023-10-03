using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum StatusType
{ 
	Slow,
	Faint,
	Knockback,
}

public class CharacterStatus : MonoBehaviour
{
	public Dictionary<StatusType, bool> CurrentStatus = new();

	[SerializeField, Range(0f, 1f)]
	private float _stopBounceTimeRatio = 0.9f;
	
	[SerializeField, Range(0f, 1f)]
	private float _bouncinesss = 0.8f;

	private List<SlowEffect> _currentSlowEffects = new();
	private FaintEffect _currentFaintEffect;
	private KnockbackEffect _currentKnockbackEffect;

	private Character _character;
	private CharacterMovement _movement;
	private Rigidbody2D _rigidbody2;
	private List<SpriteRenderer> _spriteRenderers = new();
	private List<Color> _originalColors = new();

	private float _originSpeed;

	private Coroutine _blinkCR;
	private Coroutine _faintCR;
	private Coroutine _slowCR;

	private void Awake()
	{
		_movement = GetComponent<CharacterMovement>();
		_character = GetComponent<Character>();
		_rigidbody2 = GetComponent<Rigidbody2D>();
		GetRenderers();
	}

	private void Start()
	{
		_originSpeed = _movement.ChractorMovementData.MaxSpeed;

		CurrentStatus.Add(StatusType.Slow, false);
		CurrentStatus.Add(StatusType.Faint, false);
		CurrentStatus.Add(StatusType.Knockback, false);
	}

	private void OnEnable()
	{
		Managers.Stat.onTakeDamage -= SetBlinkEffect;
		Managers.Stat.onTakeDamage += SetBlinkEffect;
		//Managers.Stat.onTakeDamage(GetComponent<Character>().playerIndex) += SetBlinkEffect;
	}

	private void Update()
	{
		ApplySlowEffect();
		ApplyFaintEffect();
		ApplyKnockbackEffect();
	}

	private void GetRenderers()
	{
		_spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_originalColors.Add(_spriteRenderers[i].color);
			Debug.Log(_spriteRenderers[i].name);
		}
	}

	#region Slow Effect
	public void SetSlowEffect(float duration, float slowRatio)
	{
		slowRatio = Mathf.Clamp(slowRatio, 0, 1); 
		_currentSlowEffects.Add(new SlowEffect(duration, slowRatio));
	}

	private void ApplySlowEffect()
	{
		if (_currentSlowEffects.Count == 0)
		{
			CurrentStatus[StatusType.Slow] = false;
			_movement.CurrentSpeed = _originSpeed;
			return;
		}

		// 활성화 된 slow 상태이상이 있는지 확인 && 가장 높은 적용 비율 가져오기
		float maxRatio = 0;
		foreach (var currentSlowEffect in _currentSlowEffects.ToList())
		{
			if (currentSlowEffect.IsEffectActive())
				maxRatio = Mathf.Max(maxRatio, currentSlowEffect.Ratio);
			else
				_currentSlowEffects.Remove(currentSlowEffect);
		}

		// slow 비율에 따라 플레이어 속도 늦추기
		if (maxRatio > 0)
		{ 
			_movement.CurrentSpeed = _originSpeed * (1 - maxRatio);
			CurrentStatus[StatusType.Slow] = true;

			if (_slowCR != null)
			{
				StopCoroutine(_slowCR); 
			}
			_slowCR = StartCoroutine(PlayEffectCo("Stat_Slow", 1, new Vector2(0,0), _character.transform.localScale.x));

		}
	}
	#endregion

	#region Faint Effect
	public void SetFaintEffect(float duration)
	{
		if (_currentFaintEffect == null)
			_currentFaintEffect = new FaintEffect(duration);
		else
			_currentFaintEffect = (duration > _currentFaintEffect.Duration) ? new FaintEffect(duration) : _currentFaintEffect;
	}

	private void ApplyFaintEffect()
	{
		if (_currentFaintEffect == null || _currentFaintEffect.IsEffectActive() == false)
		{
			CurrentStatus[StatusType.Faint] = false;
			_currentFaintEffect = null;
			return;
		}

		if (_currentFaintEffect.IsEffectActive())
		{
			CurrentStatus[StatusType.Faint] = true;

			if (_faintCR != null)
			{
				StopCoroutine(_faintCR);
			}
			_faintCR = StartCoroutine(PlayEffectCo("Stat_Faint", 1, new Vector2(0, 1), _character.transform.localScale.x));

			// 선딜 취소
			var currentSkill = (SkillBase)_character.CurrentSkill;
			if (currentSkill != null && currentSkill.IsBeforeDelay == true)
			{
				currentSkill.CancelInvoke();
			}
		}
	}
	#endregion

	#region Knockback Effect
	public void SetKnockbackEffect(float duration, float knockbackPower, Vector2 enemyPos)
	{
		if (_currentKnockbackEffect == null)
			_currentKnockbackEffect = new KnockbackEffect(duration, knockbackPower, _rigidbody2, transform.position, enemyPos);
		else
		{
			var longerDuration = (duration > _currentKnockbackEffect.LeftDuration) ? duration : _currentKnockbackEffect.LeftDuration;
			_currentKnockbackEffect = new KnockbackEffect(longerDuration, knockbackPower, _rigidbody2, transform.position, enemyPos);
		}
	}

	private void ApplyKnockbackEffect()
	{
		if (_currentKnockbackEffect == null || _currentKnockbackEffect.GetRemainingRatio() <= 0)
		{
			CurrentStatus[StatusType.Knockback] = false;
			_currentKnockbackEffect = null;
			return;
		}

		// 선딜 취소
		CurrentStatus[StatusType.Knockback] = true;
		var currentSkill = (SkillBase)_character.CurrentSkill;
		if (currentSkill != null && currentSkill.IsBeforeDelay == true)
		{
			currentSkill.CancelInvoke();
		}

		// 탄성 조절
		if (_currentKnockbackEffect.GetRemainingRatio() > 1 - _stopBounceTimeRatio)
		{
			_rigidbody2.sharedMaterial.bounciness = _bouncinesss;
		}
		else 
		{
			_rigidbody2.sharedMaterial.bounciness = 0;
		}
	}
	#endregion

	#region Blink Effect

	public void SetBlinkEffect(int index)
	{
		if (index != _character.playerIndex)
		{
			return;
		}

		if(_blinkCR != null) StopCoroutine(_blinkCR);
		_blinkCR = StartCoroutine(CR_BlinkEffect());
	}

	private IEnumerator CR_BlinkEffect()
	{
		float _blinkTime = 0.3f;

		foreach (var renderer in _spriteRenderers) 
		{
			renderer.color = Color.white;
		}
		yield return new WaitForSeconds(_blinkTime);
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_spriteRenderers[i].color = _originalColors[i];
		}
	}

	#endregion

	#region Freez Effect

	#endregion


	IEnumerator PlayEffectCo(string effName, float duration, Vector2 offset, float sign)
	{

		Transform parent = _character.transform;
		GameObject effect = Managers.Resource.Instantiate("Skills/" + effName, parent); // paraent를 character.gameObject로

		if (effect)
		{
			effect.transform.localPosition = Vector3.zero;
		}
		else
		{
			Debug.LogError($"effect is null. effName :{effName}");
		}

		effect.transform.localPosition += (Vector3)offset;
		effect.transform.localScale = new Vector3(sign * 0.2f, 0.2f, 0.2f);

		yield return new WaitForSeconds(duration); // 이펙트 재생 시간
		Managers.Resource.Release(effect);
	}
}

public class SlowEffect
{
	public float Ratio => _ratio;
	
	private float _duration;
	private float _ratio;

	public SlowEffect(float duration, float ratio)
	{
		_duration = duration;
		_ratio = ratio;
	}

	public bool IsEffectActive()
	{
		if (_duration < 0)
			return false;
		else
		{
			_duration -= Time.deltaTime;
			return true;
		}
	}
}

public class FaintEffect
{
	public float Duration { get; private set; }

	public FaintEffect(float duration)
	{
		Duration = duration;
	}

	public bool IsEffectActive()
	{
		if (Duration < 0)
			return false;
		else
		{
			Duration -= Time.deltaTime;
			return true;
		}
	}
}

public class KnockbackEffect
{
	public float LeftDuration;

	private float _originDuration;

	private const float Angle = 30f;


	public KnockbackEffect(float duration, float knockbackPower, Rigidbody2D rigidbody2D, Vector2 myPos, Vector2 enemyPos)
	{
		_originDuration = duration;
		LeftDuration = _originDuration;

		Knockback(knockbackPower, rigidbody2D, myPos, enemyPos);
	}

	private void Knockback(float knockbackPower, Rigidbody2D rigidbody2D, Vector2 myPos, Vector2 enemyPos)
	{
		float knockbackAngle = (enemyPos.x - myPos.x > 0) ? 180 - Angle : Angle;
		float angleInRadians = knockbackAngle * Mathf.Deg2Rad;
		Vector3 forceDirection = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);
		Vector3 force = forceDirection * knockbackPower;

		// 밀려나는 기능
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.AddForce(force, ForceMode2D.Impulse);
	}

	public float GetRemainingRatio()
	{
		LeftDuration -= Time.deltaTime;
		return LeftDuration / _originDuration;
	}
}



