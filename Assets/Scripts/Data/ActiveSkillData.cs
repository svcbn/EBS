using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillData : ScriptableObject
{
	[Header("OverlapBox")]
	[Header("Check")]
	[SerializeField] private Vector2 _checkBoxCenter;
	[SerializeField] private Vector2 _checkBoxSize;

	[Header("HitBox")]
	[SerializeField] private Vector2 _hitBoxCenter;
	[SerializeField] private Vector2 _hitBoxSize;

	[SerializeField] private uint _id;
	[SerializeField] private SkillType _type;
	[SerializeField] private int _priority;
	[SerializeField] private bool _isRestrictMoving;
	[SerializeField] private float _cooldown;
	[SerializeField] private float _beforeDelay;
	[SerializeField] private float _duration;
	[SerializeField] private float _afterDelay;
	[SerializeField] private float _requireMP;

	[SerializeField] private GameObject _effect;

	public Vector2 CheckBoxCenter => _checkBoxCenter;
	public Vector2 CheckBoxSize => _checkBoxSize;

	public Vector2 HitBoxCenter => _hitBoxCenter;
	public Vector2 HitBoxSize => _hitBoxSize;

	public uint Id => _id;
	public SkillType Type => _type;
	public int Priority => _priority;
	public bool IsRestrictMoving => _isRestrictMoving;
	public float Cooldown => _cooldown;
	public float BeforeDelay => _beforeDelay;
	public float Duration => _duration;
	public float AfterDelay => _afterDelay;
	public float RequireMP => _requireMP;

	public GameObject Effect => _effect;
}
