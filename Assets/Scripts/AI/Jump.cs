using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Jump : Action
{
	private CharactorJump _jump;
	private CharactorMovement _movement;

	public override void OnStart()
	{
		base.OnStart();

		_jump = GetComponent<CharactorJump>();
		_movement = GetComponent<CharactorMovement>();
	}

	public override TaskStatus OnUpdate()
	{
		_jump.OnJump(_movement.LookDirction);
		return TaskStatus.Success;
	}
}
