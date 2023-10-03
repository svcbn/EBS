using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


public class Move : Action
{
	[SerializeField] 
	private SharedFloat _arriveDistance;

	[SerializeField]
	private SharedGameObject _target;

	[SerializeField]
	private SharedBool _canMove;

	private CharacterMovement _movement;
	private CharacterJump _jump;


	public override void OnAwake()
	{
		base.OnAwake();

		_movement =  GetComponent<CharacterMovement>();
		_jump = GetComponent<CharacterJump>();
	}

	public override void OnStart()
	{
		base.OnStart();
	}

	public override TaskStatus OnUpdate()
	{
		if (Vector2.Distance(_target.Value.transform.position, transform.position) > _arriveDistance.Value
			&& _canMove.Value == true)
		{
			var directionToTarget = _target.Value.transform.position - transform.position;

			_movement.PlayerInput = directionToTarget.normalized;

			//if (GetProbabilitySuccess(1f))
			//	_jump.OnJump(_movement.PlayerInput);

			return TaskStatus.Running;
		}

		return TaskStatus.Success;
	}

	public override void OnEnd()
	{
		_movement.PlayerInput = Vector2.zero;
	}

	private bool GetProbabilitySuccess(float probability)
	{
		if (Random.Range(0, 100f) < probability)
			return true;
		else
			return false;
	}
}
