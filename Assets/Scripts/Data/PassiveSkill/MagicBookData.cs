using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(MagicBookData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(MagicBookData))]
public class MagicBookData : PassiveSkillData
{
	[SerializeField] GameObject _magicMissile;
	
	public GameObject MagicMissile => _magicMissile;
}
