using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ManaShowerData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(ManaShowerData))]
public class ManaShowerData : ActiveSkillData
{
	[SerializeField] private int _amount;

	public int Amount => _amount;
}
