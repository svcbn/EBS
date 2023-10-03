using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(KillerBootsData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(KillerBootsData))]
public class KillerBootsData : PassiveSkillData
{
	[SerializeField]
	private float _speedUpRatio;

	public float SpeedUpRatio => _speedUpRatio;
}
