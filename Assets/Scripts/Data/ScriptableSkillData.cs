using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableSkillData : ScriptableObject
{
	[SerializeField] private uint _id;
	[SerializeField] private SkillType _type;
	[SerializeField] private float _cooldown;

	public uint Id => _id;
	public SkillType Type => _type;
	public float Cooldown => _cooldown;

}
