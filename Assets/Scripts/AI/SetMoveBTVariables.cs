using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SetMoveBTVariables : Action
{
	private Character _character;

	public override void OnAwake()
	{
		base.OnAwake();

		_character = GetComponent<Character>();
		//_character.SetMoveBTVariables();
	}

	public override TaskStatus OnUpdate()
	{
		//_character.SetMoveBTVariables();
		return TaskStatus.Success;
	}

}
