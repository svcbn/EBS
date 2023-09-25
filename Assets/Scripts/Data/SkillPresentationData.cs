using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPresentationData
{
	[SerializeField]
	private Sprite _sprite;

	[SerializeField]
	private ISkill _skill;

	public Sprite Sprite => _sprite;

	public ISkill Skill
	{
		get => _skill;
		set => _skill = value;
	}
}