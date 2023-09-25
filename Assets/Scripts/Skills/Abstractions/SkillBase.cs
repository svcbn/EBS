using System.Collections;
using System.Collections.Generic;
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
				throw new System.InvalidOperationException($"Owner is already set. Owner: {_owner.name}");
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

	public virtual void Init()
	{
	}

	public virtual void Execute()
	{
	}

	public virtual bool CheckCanUse()
	{
		return true;
	}

	public virtual void OnDrawGizmos(Transform character)
	{
	}
}