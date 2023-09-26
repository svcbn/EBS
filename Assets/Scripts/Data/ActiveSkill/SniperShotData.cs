using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SniperShotData), menuName = "ScriptableObjects/ActiveSkills/" + nameof(SniperShotData))]
public class SniperShotData : ActiveSkillData
{
	[SerializeField] GameObject _hitEffect;

	public GameObject HitEffect => _hitEffect;
}
