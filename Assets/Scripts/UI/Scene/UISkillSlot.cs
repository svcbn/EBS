using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSlot : UIBase
{
	private enum Elements
	{
		Icon
	}

	private ISkill _item;

	private void Update()
	{
		
	}

	public override void Init()
	{
		Bind<GameObject, Elements>();

	}

	public void SetSkill(ISkill skill)
	{
		_item = skill;
	}
}
