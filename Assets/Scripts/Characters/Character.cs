using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
	public bool CurrentSkillIsNotNull;
	
	public List<IActiveSkill> CanUseSkills = new();
	public IActiveSkill CurrentSkill;
	public int playerIndex;
	public bool CanMove { get; private set; } = true;
	public CharacterStatus Status { get; private set; }


	[SerializeField]
	private GameObject _target;
	
	public GameObject Target => _target;

	private BehaviorTree _moveBehavior;
	private Rigidbody2D _rigidbody2;

	private readonly ObservableCollection<ISkill> _skills = new();
	
	private bool _hasCooldowmSkill;

	public IReadOnlyList<ISkill> Skills => _skills;

	public BehaviorTree BehaviorTree => _moveBehavior;

	public event NotifyCollectionChangedEventHandler CollectionChanged
	{
		add => _skills.CollectionChanged += value;
		remove => _skills.CollectionChanged -= value;
	}

	private void Awake()
	{
		//temp
		// _skills.Add(gameObject.AddComponent<Slash>());
		// _skills.Add(gameObject.AddComponent<TripleStrike>());
		// _skills.Add(gameObject.AddComponent<TeleportBack>());
		// _skills.Add(gameObject.AddComponent<Block>());
		// _skills.Add(gameObject.AddComponent<ManaShower>());
		// _skills.Add(gameObject.AddComponent<HeavyStrike>());
		// _skills.Add(gameObject.AddComponent<MagicMissile>());
		
		
		_moveBehavior = GetComponent<BehaviorTree>();
		_rigidbody2 = GetComponent<Rigidbody2D>();
		Status = GetComponent<CharacterStatus>();
	}

	private void Start()
	{
		StartCoroutine(CheckSkills());
	}

	public void AddSkill(ISkill skill)
	{
		skill.Owner = this;
		_skills.Add(skill);
	}

	private void Update()
	{
		SetMoveBTVariables();
		CurrentSkillIsNotNull = CurrentSkill is not null;
	}

	//private void OnDrawGizmos()
	//{
	//	foreach (var skill in _skills)
	//	{
	//		skill.OnDrawGizmos(transform);
	//	}
	//}

	public void UseSkill()
	{
		
	}

	private IEnumerator CheckSkills()
	{
		while (true)
		{
			CanUseSkills.Clear();
			_hasCooldowmSkill = false;
			if (CurrentSkill is { IsActing: true })
			{
				yield return new WaitForSeconds(0.05f);
				continue;
			}

			foreach (var skill in _skills.OfType<IActiveSkill>())
			{
				// TODO : 쿨다운 체크
				if (skill.IsCoolReady)
					_hasCooldowmSkill = true;

				if (skill.CheckCanUse() && CanUseSkills.Contains(skill) == false && skill.IsCoolReady)
					CanUseSkills.Add(skill);

				if (CurrentSkill != null && CurrentSkill.IsActing == false)
					CurrentSkill = null;
			}

			yield return new WaitForSeconds(0.2f);
		}
	}

	public void SetMoveBTVariables()
	{
		var direction = _target.transform.position - transform.position;
		var distance = direction.magnitude;

		_moveBehavior.SetVariableValue("Direction", direction);
		_moveBehavior.SetVariableValue("Distance", distance);
		_moveBehavior.SetVariableValue("HasCooldownSkill", _hasCooldowmSkill);
		_moveBehavior.SetVariableValue("Target", _target);

		// CanUseSkill Control
		if (CanUseSkills != null && CanUseSkills.Count > 0
			&& Status.CurrentStatus[StatusType.Faint] == false
			&& Status.CurrentStatus[StatusType.Knockback] == false)
		{
			_moveBehavior.SetVariableValue("CanUseSkill", true);
		}
		else
		{
			_moveBehavior.SetVariableValue("CanUseSkill", false);
		}

		// IsAction Control 
		if (CurrentSkill != null)
		{
			_moveBehavior.SetVariableValue("IsActing", true);
		}
		else
		{ 
			_moveBehavior.SetVariableValue("IsActing", false);
		}

		// CanMove Control
		if (CurrentSkill != null)
		{
			if (CurrentSkill.IsRestrictMoving == true
				|| Status.CurrentStatus[StatusType.Faint] == true
				|| Status.CurrentStatus[StatusType.Knockback] == true)
			{
				CanMove = false;
				_moveBehavior.SetVariableValue("CanMove", false);
			}
			else
			{
				CanMove = true;
				_moveBehavior.SetVariableValue("CanMove", true);
			}
		}
		else
		{
			if (Status.CurrentStatus[StatusType.Faint] == true
				|| Status.CurrentStatus[StatusType.Knockback] == true)
			{
				CanMove = false;
				_moveBehavior.SetVariableValue("CanMove", false);
			}
			else
			{ 
				CanMove = true;
				_moveBehavior.SetVariableValue("CanMove", true);
			}
		}

	}

	public List<IActiveSkill> GetHighPrioritySkill()
	{

		int highPriority = int.MaxValue;
		List<IActiveSkill> _tmpSkills = new();
		
		foreach( var skill in CanUseSkills)
		{
			if (skill.Priority < highPriority)
			{
				highPriority = skill.Priority;
			}
		}

		foreach( var skill in CanUseSkills)
		{
			if (skill.Priority == highPriority)
			{
				_tmpSkills.Add(skill);
			}
		}
		return _tmpSkills;
	}

	public GameObject GetTarget()
	{
		if(_target == null){
			Debug.LogWarning("Target is null");
			return null;
		}

		return _target;
	}
}
