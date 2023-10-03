using UnityEngine;

public class PassiveSkillData : ScriptableSkillData
{
	[SerializeField] private SkillType _type;

	[SerializeField] private float _amount;
	[SerializeField] private int _presentNumber;
	[SerializeField] private bool _hasPresentNumber;

	public int PresentNumber => _presentNumber;
	public bool HasPresentNumber => _hasPresentNumber;
}
