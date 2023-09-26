public class GameUIManager
{
	private UISkillSelector _skillSelector;

	public void ShowSkillSelector(SkillSelector selector)
	{
		if (_skillSelector != null)
		{
			Managers.Resource.Release(_skillSelector.gameObject);
		}

		_skillSelector = Managers.UI.ShowPopupUI<UISkillSelector>();
		_skillSelector.SetSelector(selector);
	}

	public void HideSkillSelector()
	{
		Managers.Resource.Release(_skillSelector.gameObject);
		_skillSelector = null;
	}

	public void ShowSkillList(Character left, Character right)
	{
		var list = Managers.UI.ShowSceneUI<UISkillList>();
		list.RegisterCharacter(left, right);
	}
}
