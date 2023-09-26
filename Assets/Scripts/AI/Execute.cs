using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

public class Execute : Action
{
	private Character _character;

	private IActiveSkill _selectedSkill;

	public override void OnAwake()
	{
		_character = GetComponent<Character>();
	}

	public override void OnStart()
	{
		_selectedSkill = SelectRandomSkill(_character.GetHighPrioritySkill());
		
		_selectedSkill.Init();
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
