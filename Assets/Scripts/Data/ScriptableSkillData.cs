using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableSkillData : ScriptableObject
{
	[SerializeField] private uint _id;
	[SerializeField] private SkillType _type;
	[SerializeField] private int _priority;
	[SerializeField] private bool _isRestrictMoving;
	[SerializeField] private float _cooldown;
	[SerializeField] private float _beforeDelay;
	[SerializeField] private float _afterDelay;
	[SerializeField] private float _requireMP;

	[SerializeField] private GameObject _effect;


	public uint Id => _id;
	public SkillType Type => _type;
	public int Priority => _priority;
	public bool IsRestrictMoving => _isRestrictMoving;
	public float Cooldown => _cooldown;
	public float BeforeDelay => _beforeDelay;
	public float AfterDelay => _afterDelay;
	public float RequireMP => _requireMP;

	public GameObject Effect => _effect;
}
