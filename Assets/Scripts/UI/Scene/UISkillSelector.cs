using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSelector : UIScene
{
	private enum Elements
	{
		Items,
	}

	private List<ISkill> _skills;

	public override void Init()
	{
		base.Init();

		Bind<GameObject, Elements>();
		GameObject items = Get<GameObject>((int)Elements.Items);
		foreach (Transform child in items.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}

		if (_skills != null)
		{
			foreach (var skill in _skills)
			{
				GameObject go = Managers.Resource.Instantiate("UI/Scene/UISkillSlot", items.transform);
				go.transform.localScale = Vector3.one;
				var slot = go.GetOrAddComponent<UISkillSlot>();
				slot.SetSkill(new()
				{
					Skill = skill,
				});
			}
		}
	}

	public void SetItems(List<ISkill> skills)
	{
		_skills = skills;
	}
}
