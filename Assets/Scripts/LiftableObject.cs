/*
* Filename:		LiftableObject.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/


using UnityEngine;
using System.Collections;


// Name:	LiftableObject
// Purpose:	
public class LiftableObject : MonoBehaviour
{
	// The player holding the object.
	MovingObject theHolder;
	// Where, in relation to the holder of the object, it should hover.
	float holdHeight;
	// An offset based on the item to adjust the height.
	protected float itemHoldOffset;
	// An offset to determine where the bottom of the object is.
	float objectBottomOffset;


	// Name:		Start()
	// Description:	Use this for initialization
	protected virtual void Start ()
	{
		BoxCollider2D collider = GetComponent<BoxCollider2D>();

		theHolder = null;
		holdHeight = 0;
		itemHoldOffset = 0.01f;
		// Gets an offset for where the bottom of the box collider is, in
		// relation to the player's transform location.
		objectBottomOffset = collider.offset.y - collider.size.y/2;
	}
	
	// Name:		Update()
	// Description:	Update is called once per frame
	void Update ()
	{
		if (theHolder != null)
		{
			float newX = theHolder.transform.position.x;
			float newY = 
				theHolder.transform.position.y + holdHeight;
			Vector2 newPosition = new Vector2(newX, newY);

			transform.position = newPosition;
		}
	}



	// Name:		OnLift()
	// Description:	The behaviours taken by the object when it's lifted.
	// Parameters:	MovingObject holder		- The character who lifted the object.
	//				float heightOffset		- The height at which the object is held.
	// Returns:		True if the object has been lifted. False if not. (The basic
	//				form always returns true, but this behaviour can be overriden.)
	public virtual bool OnLift(MovingObject holder, float heightOffset)
	{
		theHolder = holder;
		holdHeight = heightOffset + itemHoldOffset;

		this.GetComponent<BoxCollider2D>().enabled = false;

		GetComponent<SpriteRenderer>().sortingOrder += 1;


		// This is where the gradual lifting motion should happen...
		// (Called on theHolder?)

		return true;
	}



	// Name:		OnDrop()
	// Description:	What happens when the object is released/dropped by the holder.
	// Parameters:	Vector2 dropLocation		- The location to drop the object at.
	//				Direction releaseDirection	- What direction the object is being released in.
	public virtual bool OnDrop(Vector2 dropLocation, Direction releaseDirection)
	{
		// Whether the object can be/has been dropped.
		bool wasDropped = false;

		dropLocation.y -= objectBottomOffset;

		// Check to see if the object can be dropped
		wasDropped = hasClearance (dropLocation, releaseDirection);


		// If the object was dropped, set its location and physical properties.
		if (wasDropped)
		{
			this.GetComponent<BoxCollider2D>().enabled = true;
			transform.position = dropLocation;
			theHolder = null;
			GetComponent<SpriteRenderer>().sortingOrder -= 1;
		}

		return wasDropped;
	}



	// Name:		HasClearance()
	// Description:	Determines if this object has enough space to be dropped.
	// Parameters:	Vector2 dropLocation		- The location the object will be dropped at.
	//				Direction releaseDirection	- The direction the object is being released.
	protected virtual bool hasClearance(Vector2 dropLocation, Direction releaseDirection)
	{
		// An obstruction that might be in the way of the object being dropped.
		RaycastHit2D obstruction;
		Vector2 endPoint = dropLocation;

		bool isAllClear = false;

		// Raycast for any obstructions that might be in the way.
		obstruction = Physics2D.Linecast(dropLocation, endPoint);
		// If anything is the way, don't drop! Otherwise... do.
		if (obstruction.transform == null)
		{
			isAllClear = true;
		}

		return isAllClear;
	}
}
