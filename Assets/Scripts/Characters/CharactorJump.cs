using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharactorJump : MonoBehaviour
{
	public float jumpSpeed;
	public Vector3 velocity;
	
	CharacterGround _charaterGround;
	CharactorMovementData _charactorMovementData;
	Rigidbody2D _body;

	bool _onGround;
	bool _desiredJump;

	private void Awake()
	{
		_charaterGround = GetComponent<CharacterGround>();
		_charactorMovementData = GetComponent<CharactorMovementData>();
		_body = GetComponent<Rigidbody2D>();
	}

	public void OnJump(Vector2 lookDir)
	{

		//Create the jump, provided we are on the ground, in coyote time, or have a double jump available
		if (_onGround)
		{
			//Determine the power of the jump, based on our gravity and stats
			jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _body.gravityScale * _charactorMovementData.JumpHeight);

			if (velocity.y > 0f)
			{
				jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
			}
			else if (velocity.y < 0f)
			{
				jumpSpeed += Mathf.Abs(_body.velocity.y);
			}

			//Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
			velocity.y += jumpSpeed;
		}
	}

	private void setPhysics()
	{
		//Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
		Vector2 newGravity = new Vector2(0, (-2 * _charactorMovementData.JumpHeight) / (_charactorMovementData.TimeToJumpApex * _charactorMovementData.TimeToJumpApex));
		_body.gravityScale = (newGravity.y / Physics2D.gravity.y) * _charactorMovementData.GravMultiplier;
	}

	void Update()
	{
		setPhysics();

	}

}
