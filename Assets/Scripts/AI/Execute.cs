using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

public class Execute : Action
{
	private Character _character;

	private ISkill _selectedSkill;

	public override void OnAwake()
	{
		_character = GetComponent<Character>();
	}

	public override void OnStart()
	{
		_selectedSkill = SelectRandomSkill(_character.GetHighPrioritySkill());
	}

	public override TaskStatus OnUpdate()
	{
		if (_selectedSkill != null)
		{
			_selectedSkill.Execute();

			if (_selectedSkill.IsRestricteMoving)
			{
				_character.WaitSkillDuration(_selectedSkill.Duration);
			}
			else
			{ 
				_character.WaitSkillDuration(_selectedSkill.Duration);
			}

			return TaskStatus.Success;
		}

		return TaskStatus.Failure;
	}

	ISkill SelectRandomSkill(List<ISkill> skills)
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
