/*
 * Filename:		MovingObject.cs
 * Programmer:		Colin McMillan
 * Date:			June 2015
 * Description:		Defines the functionality of the MovingObject class, the parent of
 * 					any game object that moves via script control - players, enemies, what-have-you.
 */


using UnityEngine;
using System.Collections;


/*	Name:			Direction
 * 	Description:	An enumeration of the directions an object can be facing/heading.
 */
public enum Direction
{
	NORTH = 0,
	EAST = 1,
	SOUTH = 2,
	WEST = 3
}


/* 	Name:		MovingObject
 * 	Purpose:	Defines the behaviours of an object that moves of its own free will, so-to-speak.
 */
public abstract class MovingObject : MonoBehaviour
{
	// The directionality of the object, if applicable.
	protected Direction facing;

	// The movement speed of the object.
	public float MoveSpeed;

	// The current velocity of the object.
	protected Vector2 velocity;


	// A reference to the moving object's rigid body.
	private Rigidbody2D rigidBod;

	// A reference to the moving object's animator.
	protected Animator animateControl;


	/*	Name:			Start()
	 * 	Description:	The initializations needed when this object is created.
	 * 					Initializes variables and sets the liftable object's offsets
	 * 					as used for collision, placement, and lifting by the player.
	 */
	protected virtual void Start()
	{
		velocity = new Vector2(0, 0);

		facing = Direction.SOUTH;

		rigidBod = GetComponent<Rigidbody2D>();

		animateControl = GetComponent<Animator>();
	}



	/* 	Name:			Move()
	 * 	Description:	This object's movement method, determining where it
	 *					moves on each frame.
	 */
	protected virtual void Move()
	{
		Vector2 start = transform.position;
		Vector2 end = start + velocity;

		rigidBod.MovePosition (end);
	}
}
