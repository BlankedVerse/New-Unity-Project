/*
* Filename:		Player.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	Contains the player object, describing the controls, appearance,
* 				and behaviour of the player character.
*/


using UnityEngine;
using System.Collections;


// Name:	Player
// Purpose:	The controls, extra behaviours, and abilities of the player character,
//			as well as the player's progress and unlocked abilities/resources.
public class Player : MovingObject
{
	// Whether the player has the lifting gloves available
	static public bool hasLiftGlove = true;

	// How far away the player can grab objects from.
	public float actDistance;
	// A layer to check for grabbable objects
	public LayerMask actLayer;

	// The object the player is currently holding.
	LiftableObject heldItem;
	// The offset to use to make the item appear at the proper height.
	public float headHeight;
	// The distance between the player's position and the height they lift at.
	private float playerLiftOffset;
	// The distance between the player's position and the bottom of their collision box
	private float playerBottomOffset;


	// The strength of the player's mining power.
	static public int MineStrength = 1;
	// The player's mineral count.
	static public int MineralCount = 0;


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


		// The player shouldn't be destroyed between rounds, just disabled.
		//DontDestroyOnLoad (gameObject);
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
		// If the player is pressing the action key...
		if (Input.GetKeyDown("space"))
		{
			// If no item is held...
			if (heldItem == null)
			{
				Vector2 startPoint = LiftPoint();
				Vector2 actPoint = startPoint;
				RaycastHit2D objectHit;
				
				switch (facing)
				{
				case Direction.EAST:
					actPoint.x += actDistance;
					break;
				case Direction.WEST:
					actPoint.x -= actDistance;
					break;
				case Direction.NORTH:
					actPoint.y += actDistance;
					break;
				case Direction.SOUTH:
					actPoint.y -= actDistance;
					break;
				}

				
				// Check for interactable objects at that point
				objectHit = Physics2D.Linecast (startPoint, actPoint, actLayer);

				if (objectHit.transform != null)
				{
					// Attempt to lift it
					Lift(objectHit);
					
					// If it couldn't be lifted, try mining it!
					if (heldItem == null)
					{
						Mine(objectHit);
					}
				}
			}
			// Otherwise, put the item down.
			else
			{
				Drop();
			}
		}
	}



	// Name:		Lift()
	// Description:	Attempts to lift an object in front of the player.
	// Parameters:	RaycastHit2D objectHit	- The object to try and lift.
	private void Lift(RaycastHit2D objectHit)
	{
		// If a liftable object was hit...
		if ((objectHit.transform.tag == "Liftable")
		    || (objectHit.transform.tag == "Resource"))
		{
			float heightOffset = headHeight + GetComponent<BoxCollider2D>().offset.y;

			// Set it as the lifted object.
			heldItem = objectHit.transform.GetComponent<LiftableObject>();

			/* If the object can't be lifted - if it's a mineable rock or otherwise - 
			then reset the heldItem to be null */
			heldItem.OnLift(this, heightOffset);
			
			// This is where a lifting animation should happen.
		}
	}



	// Name:		Mine()
	// Description:	Attempts to mine an object in front of the player.
	// Parameters:	RaycastHit2D objectHit	- The object to try and mine.
	private void Mine(RaycastHit2D objectHit)
	{
		// If a mineable object was hit...
		if (objectHit.transform.tag == "Mineable")
		{
			objectHit.transform.GetComponent<MineableObject>().OnMine(MineStrength);

			// This is where a mining animation should happen (dig vs. pickaxe?)
		}
	}



	// Name:		Drop()
	// Description:	Attempts to drop an object in front of the player, if there's
	//				enough space.
	private void Drop ()
	{
		Vector2 dropLocation = DropPoint();
		
		switch (facing)
		{
		case Direction.EAST:
			dropLocation.x += actDistance;
			break;
		case Direction.WEST:
			dropLocation.x -= actDistance;
			break;
		case Direction.NORTH:
			dropLocation.y += actDistance;
			break;
		case Direction.SOUTH:
			dropLocation.y -= actDistance;
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



	// Name:		AcquireMineral()
	// Description:	Add minerals to the player's count.
	public static void AcquireMineral(int mineralValue)
	{
		MineralCount += mineralValue;
	}
}
