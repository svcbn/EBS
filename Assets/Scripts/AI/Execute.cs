using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

public class Execute : Action
{
	private Character _character;
	private CharacterMovement _movement;

	private IActiveSkill _selectedSkill;

	public override void OnAwake()
	{
		_character = GetComponent<Character>();
		_movement = GetComponent<CharacterMovement>();
	}

	public override void OnStart()
	{
		_selectedSkill = SelectRandomSkill(_character.GetHighPrioritySkill());
		
		_selectedSkill.Init();

		_movement.LookMoveDirction((_character.Target.transform.position - transform.position).normalized);
	}

	public override TaskStatus OnUpdate()
	{
		if (_selectedSkill != null)
		{
			_selectedSkill.Execute();
			_character.CurrentSkill = _selectedSkill;
			_character.CanUseSkills.Remove(_selectedSkill);

			return TaskStatus.Success;
		}

		return TaskStatus.Failure;
	}

	IActiveSkill SelectRandomSkill(List<IActiveSkill> skills)
	{
		float totalCoolTime = 0;
		foreach (var canUseSkill in skills)
		{
			totalCoolTime += canUseSkill.Cooldown;
		}

		float randomTIme = Random.Range(0, totalCoolTime);
		foreach (var canUseSkill in skills)
		{
			randomTIme -= canUseSkill.Cooldown;

			if (randomTIme <= 0)
				return canUseSkill;
		}

		return null;
	}
}
