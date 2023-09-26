using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable, CreateAssetMenu(fileName = nameof(CharactorMovementData), menuName = "ScriptableObjects/Charactors/" + nameof(CharactorMovementData))]
public class CharactorMovementData : ScriptableObject
{
	[SerializeField, Range(0f, 100f)]
	float _maxSpeed = 10f;

	[SerializeField, Range(0f, 100f)]
	float _maxAcceleration = 10f;

	public float MaxSpeed => _maxSpeed;
	public float MaxAcceleration => _maxAcceleration;
}
