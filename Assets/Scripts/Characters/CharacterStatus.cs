using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public enum StatusType
{ 
	Slow,
	Faint,
	Knockback,
	Haste,
}

public class CharacterStatus : MonoBehaviour
{
	public Dictionary<StatusType, bool> CurrentStatus = new();
	private Dictionary<StatusType, GameObject> StatusEffects = new(); 

	public float SlowRatio
	{ 
		get => _slowRatio;
		private set
		{
			_slowRatio = Mathf.Clamp(value, 0f, 1f);
		}
	}
	private float _slowRatio;

	public float HasteRatio
	{
		get => _hastRatio;
		set
		{
			_hastRatio = Mathf.Max(value, 0f);
		}
	}
	private float _hastRatio;

	public float SpeedChange
	{
		get
		{
			if (HasteRatio == 0 && SlowRatio == 0)
				return 1;
			else
				return (1 + HasteRatio) * (1 - SlowRatio);
		}
	}

	
	public float CooldownChange 
	{
		get 
		{
			return _coolDownChange;
		}
		set
		{
			_coolDownChange = value; 
		}
	}
	private float _coolDownChange = 1;

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

	private Coroutine _blinkCR;
	private Coroutine _faintCR;
	private Coroutine _slowCR;
	private Coroutine _hasteCR;

	public void Init()
	{
		SlowRatio = 0;
		HasteRatio = 0;
		CooldownChange = 1;

		foreach (var status in CurrentStatus.Keys)
		{
			CurrentStatus[status] = false;
		}
	}

	private void Awake()
	{
		_movement = GetComponent<CharacterMovement>();
		_character = GetComponent<Character>();
		_rigidbody2 = GetComponent<Rigidbody2D>();
		GetRenderers();
		
		CurrentStatus.Add(StatusType.Slow, false);
		CurrentStatus.Add(StatusType.Faint, false);
		CurrentStatus.Add(StatusType.Knockback, false);
		CurrentStatus.Add(StatusType.Haste, false);

		foreach (var effect in CurrentStatus.Keys)
		{
			var a = effect.ToString();
			GameObject go = Managers.Resource.Instantiate("Skills/Stat_" + effect.ToString(), transform);
			Vector2 offset = Resources.Load<GameObject>("Prefabs/Skills/Stat_" + effect.ToString()).transform.position;

			go.transform.localPosition = offset;
			go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

			StatusEffects.Add(effect, go);
			go.SetActive(false);
		}
	}

	private void Start()
	{
		StartCoroutine(NotifyStatusEffects());
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
		ApplyHasteEffect();
	}

	private void GetRenderers()
	{
		_spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
		for (int i = 0; i < _spriteRenderers.Count; i++)
		{
			_originalColors.Add(_spriteRenderers[i].color);
		}
	}

	IEnumerator NotifyStatusEffects()
	{
		while (true)
		{ 
			foreach (var effect in CurrentStatus.Keys)
			{ 
				if (CurrentStatus[effect] == true && StatusEffects[effect].activeSelf == false)
					StatusEffects[effect].SetActive(true);
				else if (CurrentStatus[effect] == false && StatusEffects[effect].activeSelf == true)
					StatusEffects[effect].SetActive(false);
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	#region Slow Effect
	public void SetSlowEffect(float duration, float slowRatio)
	{
		if (_character.Skills.Any(skill => skill.Id == 117))
		{
			var immuneSkill = (PassiveSkillBase)_character.Skills.First(skill => skill.Id == 117);
			immuneSkill.IsEnabled = true;

			return;
		}

		slowRatio = Mathf.Clamp(slowRatio, 0, 1); 
		_currentSlowEffects.Add(new SlowEffect(duration, slowRatio));
	}

	private void ApplySlowEffect()
	{
		// slow 활성화 된 slow 상태이상이 있는지 확인 
		if (_currentSlowEffects.Count == 0)
		{
			CurrentStatus[StatusType.Slow] = false;
			SlowRatio = 0;
			return;
		}
		CurrentStatus[StatusType.Slow] = true;

		// 가장 높은 슬로우 상태이상 적용 비율 가져오기
		foreach (var currentSlowEffect in _currentSlowEffects.ToList())
		{
			if (currentSlowEffect.IsEffectActive())
				SlowRatio = Mathf.Max(SlowRatio, currentSlowEffect.Ratio);
			else
				_currentSlowEffects.Remove(currentSlowEffect);
		}

		//// 슬로우 상태 이펙트 표시
		//if (_slowCR != null)
		//{
		//	StopCoroutine(_slowCR); 
		//}
		//_slowCR = StartCoroutine(PlayEffectCo("Stat_Slow", 1, new Vector2(0,-1), _character.transform.localScale.x));
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

			// 이펙트 표시
			//if (_faintCR != null)
			//{
			//	StopCoroutine(_faintCR);
			//}
			//_faintCR = StartCoroutine(PlayEffectCo("Stat_Faint", 1, new Vector2(0, 1), _character.transform.localScale.x));

			// 선딜 취소
			var currentSkill = (ActiveSkillBase)_character.CurrentSkill;
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
		var currentSkill = (ActiveSkillBase)_character.CurrentSkill;
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

	private void SetBlinkEffect(int index)
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

	#region Haste Effect

	private void ApplyHasteEffect()
	{
		// Haste 상태인지 확인 
		if (HasteRatio == 0)
		{
			CurrentStatus[StatusType.Haste] = false;
			return;
		}
		CurrentStatus[StatusType.Haste] = true;

		// Haste 상태 이펙트 표시
		//if (_hasteCR != null)
		//{
		//	StopCoroutine(_hasteCR);
		//}
		//_hasteCR = StartCoroutine(PlayEffectCo("Stat_Haste", 1, new Vector2(0, -1), _character.transform.localScale.x));
	}

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



