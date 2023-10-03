using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class PassiveSkillBase : MonoBehaviour, IPassiveSkill
{
	private Character _owner;

	public uint Id { get; protected set; }


	private int _presentNumber;
	private bool _isEnabled;
	
	
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

	public bool IsEnabled
	{
		get => _isEnabled;

		protected set
		{
			_isEnabled = value;
			RaisePropertyChanged();
		}
	}

	public float CurrentCooldown { get; protected set; }
	
	public bool HasPresentNumber { get; protected set; }

	public int PresentNumber
	{
		get => _presentNumber;
		
		protected set
		{
			_presentNumber = value;
			RaisePropertyChanged();
		}
	}
	
	public event PropertyChangedEventHandler PropertyChanged;
	
	public virtual void Init()
	{
	}

	protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new(propertyName));
	}
}
