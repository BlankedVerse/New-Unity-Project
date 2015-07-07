/*
* Filename:		MovingObject.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/


using UnityEngine;
using System.Collections;


public enum Direction
{
	NORTH = 0,
	EAST = 1,
	SOUTH = 2,
	WEST = 3
}


// Name:	MovingObject
// Purpose:	Defines the behaviours of an object that moves of its own free will, so-to-speak.
public abstract class MovingObject : MonoBehaviour//PhysicalPuzzleObject
{
	// The directionality of the object, if applicable.
	protected Direction facing;

	// The movement speed of the object
	public float MoveSpeed;

	// The current velocity of the object
	protected Vector2 velocity;


	// The object's rigid body
	private Rigidbody2D rigidBod;


	protected Animator animateControl;


	// Name:		Start()
	// Description:	Use this for initialization
	protected virtual void Start()
	{
		velocity = new Vector2(0, 0);

		facing = Direction.SOUTH;

		rigidBod = GetComponent<Rigidbody2D>();

		animateControl = GetComponent<Animator>();
	}



	// Name:		Move()
	// Description:	This object's movement method, determining where it
	//				moves on each frame.
	protected virtual void Move()
	{
		Vector2 start = transform.position;
		Vector2 end = start + velocity;

		rigidBod.MovePosition (end);
	}
}
