using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	private void Start()
	{
		var skillManager = new SkillManager();
		skillManager.Init();
		var selector = Managers.UI.ShowSceneUI<UISkillSelector>();
		selector.SetItems(skillManager.GetSkills());
	}

	private void Update()
	{
	}
}
