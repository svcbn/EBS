using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenu : UIPopup
{
	private enum Elements
	{
		ButtonPanel,
	}

	private string[] _menuNames = new string[] { "Restart", "Exit" };

	public override void Init()
	{
		base.Init();

		Bind<GameObject, Elements>();
		GameObject panel = Get<GameObject>((int)Elements.ButtonPanel);
		foreach (Transform child in panel.transform)
		{
			Managers.Resource.Release(child.gameObject);
		}

		SetMenuButton();
	}

	private void SetMenuButton()
	{
		GameObject panel = Get<GameObject>((int)Elements.ButtonPanel);
		foreach (int index in Enumerable.Range(0, _menuNames.Length))
		{
			var button = Managers.Resource.Instantiate("UI/Popup/BasicButton", panel.transform).GetOrAddComponent<UIButton>();
			button.SetText(_menuNames[index]);
			button.RegisterEvent(GetEvent(index));
		}
	}

	private Action<PointerEventData> GetEvent(int index)
	{
		return index switch
		{
			0 => null,
#if UNITY_EDITOR
			1 => data => UnityEditor.EditorApplication.isPlaying = false,
#else
			1 => Application.Quit(),
#endif
			_ => null
		};
	}
}