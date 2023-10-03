using System;
using System.Collections;
using System.Collections.Generic;
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

	public float Cooldown { get; protected set; }
	public float CurrentCooldown { get; protected set; }

	public virtual void Init()
	{
	}
	
	public virtual void Reset()
	{
	}

	protected T LoadData<T>()
		where T : ScriptableSkillData
	{
		T data = Managers.Resource.Load<T>($"Data/{typeof(T).Name}");
		
		if (data == null)
		{
			Debug.LogWarning($"{name} is not found");
			return null;
		}

		return data;
	}
}