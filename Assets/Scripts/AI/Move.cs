using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class Move : Action
{
	[SerializeField] 
	private SharedFloat _arriveDistance;

	[SerializeField]
	private SharedGameObject _target;
	//private SharedVector3 _directionToTarget;
	
	private CharactorMovement _movement;

	public override void OnAwake()
	{
		base.OnAwake();

		_movement =  GetComponent<CharactorMovement>();
	}

	public override void OnStart()
	{
		base.OnStart();
	}

	public override TaskStatus OnUpdate()
	{
		if (Vector2.Distance(_target.Value.transform.position, transform.position) > _arriveDistance.Value)
		{
			var directionToTarget = _target.Value.transform.position - transform.position;

			_movement.PlayerInput = directionToTarget.normalized;
			return TaskStatus.Running;
		}

		_movement.PlayerInput = Vector2.zero;
		return TaskStatus.Success;
	}
}
