using UnityEngine;

public class Test : MonoBehaviour
{
	private UISkillSelector _selector;

	private void Awake()
	{
		
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			GameManager.Instance.ChangeState(GameManager.GameState.PickSkill);
		}
#endif
	}
}
