using System;

public class GameUIManager
{
	private UISkillSelector _skillSelector;

	private UIMenu _menu;

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

	public void ShowTitle(Action onStart)
	{
		Managers.UI.ClearAllPopup();

		var title = Managers.UI.ShowPopupUI<UITitle>();
		title.StartButtonClicked += () => Managers.UI.ClosePopupUI(title);
		title.StartButtonClicked += onStart;
	}

	public void ShowMenu()
	{
		if (_menu != null)
		{
			Managers.UI.ClosePopupUI(_menu);
			_menu = null;
		}

		_menu = Managers.UI.ShowPopupUI<UIMenu>();
	}
	
	public void SetSkillPresenter(Character character)
	{
		var presenter = Managers.UI.ShowSceneUI<UISkillPresenter>();
		presenter.SetSkill(character);
	}
}
