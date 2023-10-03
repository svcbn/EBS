using UnityEngine;

public class PassiveSkillData : ScriptableSkillData
{
	[SerializeField] private SkillType _type;

	[SerializeField] private float _amount;

	public float Amount => _amount;
}
