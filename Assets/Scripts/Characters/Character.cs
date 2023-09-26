using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
	public List<IActiveSkill> CanUseSkills = new();
	public IActiveSkill CurrentSkill;

	[SerializeField]
	private int _hp;

	[SerializeField]
	private GameObject _target;

	private BehaviorTree _moveBehavior;
	private Rigidbody2D _rigidbody2;

	private List<ISkill> _skills = new();
	
	private bool _hasCooldowmSkill;



	private void Awake()
	{
		//temp
		_skills.Add(gameObject.AddComponent<Slash>());
		_skills.Add(gameObject.AddComponent<TripleStrike>());
		//_skills.Add(gameObject.AddComponent<TeleportBack>());
		foreach (var skill in _skills)
		{
			skill.Init();
			skill.Owner = this;
		}
		
		_moveBehavior = GetComponent<BehaviorTree>();
		_rigidbody2 = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		StartCoroutine(CheckSkills());

		// test
		foreach (var skill in _skills)
		{ 
			skill.Owner = this;	
		}
	}

	public void AddSkill(ISkill skill)
	{
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

	public void TakeDamage(int damage)
	{
		_hp -= damage;
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

				if (skill.CheckCanUse() && CanUseSkills.Contains(skill) == false)
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

		if (CanUseSkills != null && CanUseSkills.Count > 0)
			_moveBehavior.SetVariableValue("CanUseSkill", true);
		else
			_moveBehavior.SetVariableValue("CanUseSkill", false);

		if (CurrentSkill != null)
		{
			_moveBehavior.SetVariableValue("IsActing", true);


			if (CurrentSkill.IsRestrictMoving == true)
				_moveBehavior.SetVariableValue("CanMove", false);
			else
				_moveBehavior.SetVariableValue("CanMove", true);
		}
		else
		{ 
			_moveBehavior.SetVariableValue("IsActing", false);
			_moveBehavior.SetVariableValue("CanMove", true);
		}

		_moveBehavior.SetVariableValue("Direction", direction);
		_moveBehavior.SetVariableValue("Distance", distance);
		_moveBehavior.SetVariableValue("HasCooldownSkill", _hasCooldowmSkill);
		_moveBehavior.SetVariableValue("Target", _target);
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
