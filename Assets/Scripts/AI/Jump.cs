using BehaviorDesigner.Runtime.Tasks;

public class Jump : Action
{
	private CharactorJump _jump;
	private CharacterMovement _movement;

	public override void OnStart()
	{
		base.OnStart();

		_jump = GetComponent<CharactorJump>();
		_movement = GetComponent<CharacterMovement>();
	}

	public override TaskStatus OnUpdate()
	{
		_jump.OnJump(_movement.LookDirction);
		return TaskStatus.Success;
	}
}
