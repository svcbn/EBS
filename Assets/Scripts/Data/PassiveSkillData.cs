using UnityEngine;

public class PassiveSkillData : ScriptableSkillData
{
	[SerializeField] private int _amount;

	public int Amount => _amount;
}
