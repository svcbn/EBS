using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableSkillData : ScriptableObject
{
	[SerializeField] private uint _id;
	[SerializeField] private SkillType _type;

	public uint Id => _id;
	public SkillType Type => _type;

}
