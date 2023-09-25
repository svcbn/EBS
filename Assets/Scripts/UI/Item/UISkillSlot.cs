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

	private static readonly Color s_SelectedColor = new(1, 0, 0, 0.5f);

	private static readonly Color s_UnselectedColor = new(1, 1, 1, 0.5f);

	private static readonly float s_SelectedScale = 1.2f;

	private SkillPresentationData _item;

	private Image _border;

	protected override void Start()
	{
		base.Start();

		_border = GetComponent<Image>();
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

	public void Select()
	{
		_border.color = s_UnselectedColor;
	}

	public void Unselect()
	{
		_border.color = s_SelectedColor;
	}

	private void SetSkillInfo()
	{
		var iconRoot = Get<GameObject>((int)Elements.Icon);
		if (iconRoot.TryGetComponent<Image>(out var icon))
		{
			// TODO : 아이콘 이미지 변경
			if (_item.Info != null)
			{
				icon.sprite = _item.Info.Sprite;
			}
		}
	}
}
