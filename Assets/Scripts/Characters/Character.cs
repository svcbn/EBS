using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
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
	private bool _canUseSkill = true;

	private void Awake()
	{
		_moveBehavior = GetComponent<BehaviorTree>();
	}

	private void Start()
	{
		_moveBehavior.SetVariableValue("CanUseSkill", _canUseSkill);
		
	}

	private void Update()
	{
		var direction = _target.transform.position - transform.position;
		var distance = direction.magnitude;

		_moveBehavior.SetVariableValue("Direction", direction);
		_moveBehavior.SetVariableValue("Distance", distance);

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
}
