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

	public override void OnAwake()
	{
		base.OnAwake();

		_movement =  GetComponent<CharacterMovement>();
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
			return TaskStatus.Running;
		}

		_movement.PlayerInput = Vector2.zero;
		return TaskStatus.Success;
	}
}
