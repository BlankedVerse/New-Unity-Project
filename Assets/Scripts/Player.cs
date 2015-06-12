/*
* Filename:		Player.cs
* Programmer:	Colin McMillan
* Date:			
* Description:	
*/


using UnityEngine;
using System.Collections;


// Name:	Player
// Purpose:	
public class Player : MovingObject
{
	// Name:		Start()
	// Description:	Use this for initialization
	protected override void Start()
	{
		base.Start();
	}
	
	// Name:		Update()
	// Description:	Update is called once per frame
	private void Update()
	{
		int horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		int vertical = (int) (Input.GetAxisRaw ("Vertical"));

		Direction lastFacing = facing;


		// If the player is walking in any direction...
		if (vertical != 0 || horizontal != 0)
		{
			animateControl.SetBool ("IsWalking", true);
		}
		else
		{
			animateControl.SetBool ("IsWalking", false);
		}

		// Use the input to determine the character's facing
		if (vertical > 0)
		{
			facing = Direction.NORTH;
		}
		else if (vertical < 0)
		{
			facing = Direction.SOUTH;
		}
		else if (horizontal > 0)
		{
			facing = Direction.EAST;
		}
		else if (horizontal < 0)
		{
			facing = Direction.WEST;
		}


		// If the facing has changed...
		if (lastFacing != facing)
		{
			animateControl.SetInteger("FacingDirection", (int) facing);
		}


		velocity.x = horizontal * MoveSpeed;
		velocity.y = vertical * MoveSpeed;

		Move();
	}
}
