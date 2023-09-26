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
			GameManager.Instance.ChangeState(GameManager.GameState.PickSkill);
		}
	}
}
