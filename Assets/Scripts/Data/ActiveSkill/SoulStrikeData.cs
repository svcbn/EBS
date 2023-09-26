using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SoulStrikeData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(SoulStrikeData))]
public class SoulStrikeData : ActiveSkillData
{
	[SerializeField] private int _amount;

	public int Amount => _amount;
}
