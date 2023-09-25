using System;
using System.Collections;
using UnityEngine;

public abstract class SkillBase : ISkill
{
	private Character _owner;

	public uint Id { get; protected set; }

	public Character Owner
	{
		get => _owner;
		set
		{
			if (_owner != null && _owner != value)
			{
				throw new InvalidOperationException($"Owner is already set. Owner: {_owner.name}");
			}

			_owner = value;
		}
	}

	public SkillType Type { get; protected set; }

	public int Priority { get; protected set; }

	public bool IsRestricteMoving { get; protected set; }

	public float Cooldown { get; protected set; }

	public float BeforeDelay { get; protected set; }

	public float Duration { get; protected set; }

	public float AfterDelay { get; protected set; }

	public bool IsCoolReady { get; protected set; }

	public bool IsActing { get; protected set; }

	public virtual void Init()
	{
	}

	public virtual void Execute()
	{
		IsCoolReady = false;
		IsActing = true;
	}

	public virtual bool CheckCanUse()
	{
		return true;
	}

	public virtual void OnDrawGizmos(Transform character)
	{
	}

	protected virtual void CalculateCooltime()
	{
		Owner.StartCoroutine(CoCalculateTime(Cooldown, () => IsCoolReady = true));
		float delay = BeforeDelay + Duration + AfterDelay;
		Owner.StartCoroutine(CoCalculateTime(delay, () => IsActing = false));
	}

	private static IEnumerator CoCalculateTime(float time, Action onFinish)
	{
		const float waitTime = 0.1f;

		float timer = 0;
		while (timer < time)
		{
			yield return new WaitForSeconds(waitTime);
			timer += waitTime;
		}

		onFinish?.Invoke();
	}
}