using UnityEngine;

public class PassiveSkillData : ScriptableSkillData
{
	[SerializeField] private float _amount;

	public float Amount => _amount;
}
