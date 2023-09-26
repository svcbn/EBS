using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
	public List<IActiveSkill> CanUseSkills = new();
	public IActiveSkill CurrentSkill;
	public int playerIndex;

	[SerializeField]
	private GameObject _target;
	
	public GameObject Target => _target;

	private BehaviorTree _moveBehavior;
	private Rigidbody2D _rigidbody2;
	private CharacterStatus _status;

	private readonly ObservableCollection<ISkill> _skills = new();
	
	private bool _hasCooldowmSkill;

	public IReadOnlyList<ISkill> Skills => _skills;

	public event NotifyCollectionChangedEventHandler CollectionChanged
	{
		add => _skills.CollectionChanged += value;
		remove => _skills.CollectionChanged -= value;
	}

	private void Awake()
	{		
		_moveBehavior = GetComponent<BehaviorTree>();
		_rigidbody2 = GetComponent<Rigidbody2D>();
		_status = GetComponent<CharacterStatus>();
	}

	private void Start()
	{
		StartCoroutine(CheckSkills());



		// test
		GameManager.Skill.GetSkill(1, this);
		foreach (var skill in _skills)
		{ 
			skill.Owner = this;	
		}
	}

	public void AddSkill(ISkill skill)
	{
		skill.Owner = this;
		_skills.Add(skill);
	}

	private void Update()
	{
		SetMoveBTVariables();

		if (Input.GetKeyDown(KeyCode.Space)) // for test
		{

			if( name != "Capsule A") { return; } 

			CurrentSkill.Init();
			CurrentSkill.Owner = this;
			CurrentSkill.Execute();
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			//AddForce
		}


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
			&& _status.CurrentStatus[StatusType.Faint] == false)
		{ 
			_moveBehavior.SetVariableValue("CanUseSkill", true);
		}
		else
			_moveBehavior.SetVariableValue("CanUseSkill", false);

		// IsAction and CanMove Control
		if (CurrentSkill != null)
		{
			_moveBehavior.SetVariableValue("IsActing", true);


			if (CurrentSkill.IsRestrictMoving == true || _status.CurrentStatus[StatusType.Faint] == true)
				_moveBehavior.SetVariableValue("CanMove", false);
			else
				_moveBehavior.SetVariableValue("CanMove", true);
		}
		else
		{ 
			_moveBehavior.SetVariableValue("IsActing", false);


			if (_status.CurrentStatus[StatusType.Faint] == true)
				_moveBehavior.SetVariableValue("CanMove", false);
			else
				_moveBehavior.SetVariableValue("CanMove", true);
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
