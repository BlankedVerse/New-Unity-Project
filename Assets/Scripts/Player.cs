/*
* Filename:		Player.cs
* Programmer:	Colin McMillan
* Date:			
* Description:	
*/


using UnityEngine;
using System.Collections;


// Name:	Player
// Purpose:	s
public class Player : MovingObject
{
	// Whether the player has the lifting gloves available
	public bool hasLiftGlove;
	// Whether the player has the boomerang available to throw.
	public bool hasBoomerang;

	// How far away the player can grab objects from.
	public float grabDistance;
	// A layer to check for grabbable objects
	public LayerMask grabLayer;

	// The object the player is currently holding.
	LiftableObject heldItem;
	// The offset to use to make the item appear at the proper height.
	public float headHeight;
	private float playerLiftOffset;
	private float playerBottomOffset;


	// Name:		Start()
	// Description:	Use this for initialization
	protected override void Start()
	{
		BoxCollider2D collider = GetComponent<BoxCollider2D>();

		base.Start();

		heldItem = null;

		// Gets the midpoint of the player's collision box, to determine
		// where they lift from.
		playerLiftOffset = collider.offset.y;
		// Gets an offset for where the bottom of the box collider is, in
		// relation to the player's transform location.
		playerBottomOffset = collider.offset.y - collider.size.y/2;
	}
	
	// Name:		Update()
	// Description:	Update is called once per frame
	private void Update()
	{
		// Process movement input/motion
		Move();
		Act();
	}





	// Name:		Move()
	// Description:	Processes user input to determine the direction
	//				and movement for this frame.
	protected override void Move()
	{
		// Get control input for movement and tool usage.
		int horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		int vertical = (int) (Input.GetAxisRaw ("Vertical"));
		
		Direction lastFacing = facing;
		
		
		// If the player is walking in any direction...
		if (vertical != 0 || horizontal != 0)
		{
			// Set the animation switch "IsWalking"
			animateControl.SetBool ("IsWalking", true);
		}
		else
		{
			animateControl.SetBool ("IsWalking", false);
		}
		
		// Use the input to determine the character's facing
		if (horizontal > 0)
		{
			facing = Direction.EAST;
		}
		else if (horizontal < 0)
		{
			facing = Direction.WEST;
		}
		else if (vertical > 0)
		{
			facing = Direction.NORTH;
		}
		else if (vertical < 0)
		{
			facing = Direction.SOUTH;
		}
		
		
		// If the facing has changed...
		if (lastFacing != facing)
		{
			animateControl.SetInteger("FacingDirection", (int) facing);
		}
		
		
		velocity.x = horizontal * MoveSpeed;
		velocity.y = vertical * MoveSpeed;

		base.Move();
	}



	// Name:		Act()
	// Description:	Goes through the various actions available to the player,
	//				depending on the items/tools they have.
	private void Act()
	{
		// If the player has the glove and hits the space bar.
		if ((hasLiftGlove) && (Input.GetKeyDown("space")))
		{
			LiftDrop();
		}
	}



	// Name:		LiftDrop()
	// Description: Attempts a lift action in the direction the
	//				player is facing, if they aren't holding an item.
	//				If the player IS holding an item, the item is dropped
	//				in the appropriate direction.
	private void LiftDrop()
	{
		// If no item is held...
		if (heldItem == null)
		{
			Lift();
		}
		// Otherwise, put the item down.
		else
		{
			Drop();
		}
	}



	// Name:		Lift()
	// Description:	Attempts to lift an object in front of the player.
	private void Lift()
	{
		Vector2 startPoint = LiftPoint();
		Vector2 grabPoint = startPoint;
		RaycastHit2D objectHit;
		
		switch (facing)
		{
		case Direction.EAST:
			grabPoint.x += grabDistance;
			break;
		case Direction.WEST:
			grabPoint.x -= grabDistance;
			break;
		case Direction.NORTH:
			grabPoint.y += grabDistance;
			break;
		case Direction.SOUTH:
			grabPoint.y -= grabDistance;
			break;
		}
		Debug.Log(grabPoint);
		
		// Check for liftable objects at that point
		objectHit = Physics2D.Linecast (startPoint, grabPoint, grabLayer);
		
		
		// If a liftable object was hit...
		if (objectHit.transform != null)
		{
			float heightOffset = headHeight + GetComponent<BoxCollider2D>().offset.y;
			
			Debug.Log(objectHit.ToString());
			// Set it as the lifted object.
			heldItem = objectHit.transform.GetComponent<LiftableObject>();
			
			heldItem.OnLift(this, heightOffset);
			
			// This is where a lifting animation should happen.
		}
	}



	// Name:		Drop()
	// Description:	Attempts to drop an object in front of the player, if there's
	//				enough space.
	private void Drop()
	{
		Vector2 dropLocation = DropPoint();
		
		switch (facing)
		{
		case Direction.EAST:
			dropLocation.x += grabDistance;
			break;
		case Direction.WEST:
			dropLocation.x -= grabDistance;
			break;
		case Direction.NORTH:
			dropLocation.y += grabDistance;
			break;
		case Direction.SOUTH:
			dropLocation.y -= grabDistance;
			break;
		}
		
		// Attempt to drop the item. If successful, the player is no longer holding the item.
		if (heldItem.OnDrop(dropLocation, facing))
		{
			heldItem = null;
		}
	}



	// Name:		LiftPoint()
	// Description:	Calculates where the lifting action should originate from.
	//				That is, the starting point for checking for liftable objects.
	//				Presently, this is the midpoint of the player's hitbox.
	// Return:		A Vector2 indicating 
	private Vector2 LiftPoint()
	{
		float X = transform.position.x;
		float Y = transform.position.y;

		Y += playerLiftOffset;

		return new Vector2(X, Y);
	}
	
	
	
	// Name:		LiftPoint()
	// Description:	Calculates where objects should be dropped off at.
	// Return:		A Vector2 indicating 
	private Vector2 DropPoint()
	{
		float X = transform.position.x;
		float Y = transform.position.y;
		
		Y += playerBottomOffset;
		
		return new Vector2(X, Y);
	}
}
