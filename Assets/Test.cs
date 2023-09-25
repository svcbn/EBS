using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	private void Awake()
	{
		var skillManager = new SkillManager();
		skillManager.Init();
		var selector = Managers.UI.ShowSceneUI<UISkillSelector>();
		selector.SetItems(skillManager.GeneratePool(9));
	}

	private void Update()
	{
	}
}
