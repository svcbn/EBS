using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager
{
	private UISkillSelector _skillSelector;

	public void ShowSkillSelector(List<SkillInfo> skills)
	{
		if (_skillSelector != null)
		{
			Managers.Resource.Release(_skillSelector.gameObject);
		}

		_skillSelector = Managers.UI.ShowSceneUI<UISkillSelector>();
		_skillSelector.SetItems(skills);
	}

	public void HideSkillSelector()
	{
		Managers.Resource.Release(_skillSelector.gameObject);
		_skillSelector = null;
	}
}
