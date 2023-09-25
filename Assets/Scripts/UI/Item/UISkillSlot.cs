using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : UIBase
{
	private enum Elements
	{
		Icon
	}

	private SkillPresentationData _item;

	private void Update()
	{
		
	}

	public override void Init()
	{
		Bind<GameObject, Elements>();

		if (_item != null)
		{
			SetSkillInfo();
		}
	}

	public void SetSkill(SkillPresentationData skill)
	{
		_item = skill;
	}

	private void SetSkillInfo()
	{
		var iconRoot = Get<GameObject>((int)Elements.Icon);
		if (iconRoot.TryGetComponent<Image>(out var icon))
		{
			// TODO : 아이콘 이미지 변경
			icon.sprite = _item.Sprite;
		}
	}
}
