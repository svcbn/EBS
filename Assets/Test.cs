using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	private UISkillSelector _selector;

	private void Awake()
	{
		
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_selector == null)
			{
				var skillManager = new SkillManager();
				skillManager.Init();
				var selector = new SkillSelector(skillManager.GeneratePool(9));
				_selector = Managers.UI.ShowPopupUI<UISkillSelector>();
				_selector.SetSelector(selector);
			}
			else
			{
				Managers.UI.ClosePopupUI(_selector);
				_selector = null;
			}
		}
	}
}
