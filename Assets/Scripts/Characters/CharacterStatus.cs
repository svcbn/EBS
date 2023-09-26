using System;
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

	private List<SlowEffect> _currentSlowEffects = new();

	private FaintEffect _currentFaintEffect;

	private CharacterMovement _movement;

	private Character _character;

	private float _originSpeed;

	private void Awake()
	{
		_movement = GetComponent<CharacterMovement>();
		_character = GetComponent<Character>();
	}

	private void Start()
	{
		_originSpeed = _movement.ChractorMovementData.MaxSpeed;

		CurrentStatus.Add(StatusType.Slow, false);
		CurrentStatus.Add(StatusType.Faint, false);
	}

	private void Update()
	{
		ApplySlowEffect();
		ApplyFaintEffect();
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
		}
	}
	#endregion

	#region Faint Effect
	public void SetFaintEffect(float duration)
	{
		if (_currentFaintEffect == null)
		{
			_currentFaintEffect = new FaintEffect(duration);
		}
		else
		{
			_currentFaintEffect = (duration > _currentFaintEffect.Duration) ? new FaintEffect(duration) : _currentFaintEffect;
		}
	}

	private void ApplyFaintEffect()
	{
		if (_currentFaintEffect == null || _currentFaintEffect.IsEffectActive() == false)
		{
			CurrentStatus[StatusType.Faint] = false;
			return;
		}

		if (_currentFaintEffect.IsEffectActive())
		{
			CurrentStatus[StatusType.Faint] = true;

			// 선딜 취소
			var currentSkill = (SkillBase)_character.CurrentSkill;
			if (currentSkill != null && currentSkill.IsBeforeDelay == true)
			{
				currentSkill.CancelInvoke();
			}
		}
	}
	#endregion
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

