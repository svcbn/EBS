using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillSelector : UIScene
{
	private enum Elements
	{
		Items,
	}

	public override void Init()
	{
		base.Init();

		Bind<GameObject, Elements>();
		GameObject items = Get<GameObject>((int)Elements.Items);
		foreach (Transform child in items.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}
	}

	public void SetItems(List<ISkill> skills)
	{
		GameObject items = Get<GameObject>((int)Elements.Items);
		foreach (var skill in skills)
		{
			GameObject go = Managers.Resource.Instantiate("UI/Skills/SkillSlot", items.transform);
		}
	}
}
