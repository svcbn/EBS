using UnityEngine;

public class CharacterJump : MonoBehaviour
{
	public Vector3 velocity;
	
	CharacterGround _charaterGround;
	CharacterMovement _charactorMovement;
	CharacterMovementData _charactorMovementData;
	CharacterStatus _status;
	Rigidbody2D _body;

	bool _desiredJump;

	private void Awake()
	{
		_charaterGround = GetComponent<CharacterGround>();
		_charactorMovement = GetComponent<CharacterMovement>();
		_charactorMovementData = _charactorMovement.ChractorMovementData;
		_status = GetComponent<CharacterStatus>();
		_body = GetComponent<Rigidbody2D>();
	}

	public void OnJump(Vector2 lookDir)
	{
		if (_charaterGround.GetOnGround())
		{
			_body.velocity = new Vector2(_body.velocity.x, 0);

			if (_status.CurrentStatus[StatusType.Slow] == true)
				_body.AddForce(Vector2.up * _charactorMovementData.JumpPower * (1 - _status.SlowRatio), ForceMode2D.Impulse);
			else
				_body.AddForce(Vector2.up * _charactorMovementData.JumpPower, ForceMode2D.Impulse);
		}
	}

	private void setPhysics()
	{
		Vector2 newGravity = new Vector2(0, (-2 * _charactorMovementData.JumpHeight) / (_charactorMovementData.TimeToJumpApex * _charactorMovementData.TimeToJumpApex));
		_body.gravityScale = (newGravity.y / Physics2D.gravity.y) * _charactorMovementData.GravMultiplier;
	}
}
