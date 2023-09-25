using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum MoveType
{ 
	Move,
	Flee,
}

public class Move : Action
{
	[SerializeField] 
	private SharedFloat _arriveDistance;

	[SerializeField]
	private SharedVector3 _directionToTarget;

	[SerializeField]
	private MoveType _moveType;

	private float _moveSpeed = 2; 

	public override TaskStatus OnUpdate()
	{
		switch (_moveType)
		{
			case MoveType.Move:
				{
					if (Vector2.Distance(_directionToTarget.Value, transform.position) > _arriveDistance.Value)
					{
						MoveTo(_directionToTarget.Value, _moveType);
						return TaskStatus.Running;
					}
				}
				break;
			case MoveType.Flee:
				if (Vector2.Distance(_directionToTarget.Value, transform.position) < _arriveDistance.Value)
				{
					MoveTo(_directionToTarget.Value, _moveType);
					return TaskStatus.Running;
				}
				break;
		}

		return TaskStatus.Failure;
	}

	void MoveTo(Vector2 directionToTarget, MoveType moveType = 0)
	{
		switch (moveType)
		{ 
			case MoveType.Move:
				transform.position += (Vector3)directionToTarget * Time.deltaTime * _moveSpeed;
				break;
			case MoveType.Flee:
				transform.position -= (Vector3)directionToTarget * Time.deltaTime * _moveSpeed;
				break;

		}
	}

}
