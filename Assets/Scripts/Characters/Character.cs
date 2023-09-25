using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
	public bool IsActing { get; set; } 
	public List<ISkill> CanUseSkills { get; private set; }


	[SerializeField]
	private int _hp;

	[SerializeField]
	private GameObject _target;

	private BehaviorTree _moveBehavior;

	private List<ISkill> _skills = new()
	{
		new Slash(),
	};

	
	private ISkill _currentSkill;

	private void Awake()
	{
		_moveBehavior = GetComponent<BehaviorTree>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		SetMoveBTVariables();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_currentSkill = _skills.FirstOrDefault();
			_currentSkill.Init();
			_currentSkill.Owner = this;
			_currentSkill.Execute();
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
			foreach (var skill in _skills)
			{
				// TODO : 쿨다운 체크
			}

			yield return new WaitForSeconds(0.2f);
		}
	}

	private void SetMoveBTVariables()
	{
		var direction = _target.transform.position - transform.position;
		var distance = direction.magnitude;

		if (CanUseSkills.Count > 0)
			_moveBehavior.SetVariableValue("CanUseSkill", true);
		else
			_moveBehavior.SetVariableValue("CanUseSkill", false);

		_moveBehavior.SetVariableValue("Direction", direction);
		_moveBehavior.SetVariableValue("Distance", distance);
		_moveBehavior.SetVariableValue("IsActing", IsActing);
	}

	public void WaitSkillDuration(float skillDuration)
	{
		IsActing = true;

		StartCoroutine(nameof(OutSkillDuration), skillDuration);
	}

	IEnumerator OutSkillDuration(float skillDuration)
	{
		if (IsActing == false)
			yield break;

		yield return new WaitForSeconds(skillDuration);
		IsActing = false;
	}
}
