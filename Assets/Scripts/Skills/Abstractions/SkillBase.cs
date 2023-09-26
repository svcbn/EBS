using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
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

	public bool IsRestrictMoving { get; protected set; }

	public float Cooldown { get; protected set; }

	public float BeforeDelay { get; protected set; }

	public float Duration { get; protected set; }

	public float AfterDelay { get; protected set; }

	public int RequireMP { get; protected set; }

	public bool IsCoolReady { get; protected set; } = true;

	public bool IsActing { get; protected set; }

	public virtual void Init()
	{
	}

	public virtual void Execute()
	{
		Debug.Log(GetType().Name);

		IsCoolReady = false;
		IsActing = true;

		CalculateCooltime();
	}

	public abstract bool CheckCanUse();

	protected virtual void CalculateCooltime()
	{
		Owner.StartCoroutine(CoCalculateTime(Cooldown, () => IsCoolReady = true));
		float delay = BeforeDelay + Duration + AfterDelay;
		Owner.StartCoroutine(CoCalculateTime(delay, () => IsActing = false));
	}

	protected virtual bool CheckEnemyInBox(Vector2 center, Vector2 size)
	{
		float x = Owner.transform.localScale.x < 0 ? -1 : 1;
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * center.x, center.y);
		var boxes = Physics2D.OverlapBoxAll(centerInWorld, size, 0);
		bool flag = boxes.Length != 0;
		return boxes.Any(box => box.TryGetComponent<Character>(out var character) && character != Owner);
	}

	protected virtual bool CheckEnoughMP(int _mp)
	{
		// float mp = owner의 mp 얻어오기
		// _mp와 owner의 현재 mp 비교
		return true;
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