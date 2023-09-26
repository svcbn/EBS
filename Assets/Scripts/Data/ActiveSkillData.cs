using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillData : ScriptableSkillData
{
	[Header("OverlapBox")]
	[Header("Check")]
	[SerializeField] private Vector2 _checkBoxCenter;
	[SerializeField] private Vector2 _checkBoxSize;

	[Header("HitBox")]
	[SerializeField] private Vector2 _hitBoxCenter;
	[SerializeField] private Vector2 _hitBoxSize;

	
	[SerializeField] private float _damage;


	public Vector2 CheckBoxCenter => _checkBoxCenter;
	public Vector2 CheckBoxSize => _checkBoxSize;

	public Vector2 HitBoxCenter => _hitBoxCenter;
	public Vector2 HitBoxSize => _hitBoxSize;

	
	public float Damage => _damage;

}
