using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

public class Execute : Action
{
	private Character _character;

	private ISkill _selectedSkill;

	private void Awake()
	{
		_character = GetComponent<Character>();
	}

	private void Start()
	{
		_selectedSkill = SelectRandomSkill(_character.CanUseSkills);
	}

	public override TaskStatus OnUpdate()
	{
		if (_selectedSkill != null)
		{
			_selectedSkill.Execute();


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
