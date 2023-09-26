using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatusType
{ 
	Slow,
	faint,
	Knockback,
}

public class CharactorStatus : MonoBehaviour
{
	private Dictionary<StatusType, bool> _currentStatus;

	private List<SlowEffect> _currentSlowEffects = new();

	private CharactorMovement _movement;

	private float _originSpeed;

	private void Awake()
	{
		_movement = GetComponent<CharactorMovement>();
	}

	private void Start()
	{
		_originSpeed = _movement.ChractorMovementData.MaxSpeed;
	}

	private void Update()
	{
		ApplySlowEffect();
	}

	public void SetSlowEffect(float duration, float slowRatio)
	{
		slowRatio = Mathf.Clamp(slowRatio, 0, 1); 
		_currentSlowEffects.Add(new SlowEffect(duration, slowRatio));
	}

	private void ApplySlowEffect()
	{
		if (_currentSlowEffects.Count == 0)
		{
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
			_movement.CurrentSpeed = _originSpeed * (1 - maxRatio);
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
