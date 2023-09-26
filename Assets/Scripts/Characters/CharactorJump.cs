using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharactorJump : MonoBehaviour
{
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

	public void OnJump(InputAction.CallbackContext context)
	{
		if (_onGround)
		{
			_desiredJump = true;
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

		
		//if (_desiredJump)
		//{

		//	if (jumpBufferCounter > jumpBuffer)
		//	{
		//		//If time exceeds the jump buffer, turn off "desireJump"
		//		_desiredJump = false;
		//		jumpBufferCounter = 0;
		//	}
		//}

		////If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
		////So, start the coyote time counter...
		//if (!currentlyJumping && !onGround)
		//{
		//	coyoteTimeCounter += Time.deltaTime;
		//}
		//else
		//{
		//	//Reset it when we touch the ground, or jump
		//	coyoteTimeCounter = 0;
		//}
	}
}
