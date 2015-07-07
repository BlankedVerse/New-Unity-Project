/*
* Filename:		Key.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/


using UnityEngine;
using System.Collections;


// Name:	Key
// Purpose:	Defines a key, and its ability to unlock objects it is used on.
public class Key : LiftableObject
{
	protected override void Start ()
	{
		base.Start();
		//itemHoldOffset = -.05f;
	}



	// Name:		UseKey()
	// Description:	Use up the key after it's been used to unlock something
	public virtual void UseKey ()
	{
		gameObject.SetActive(false);
	}



	// Name:		OnTriggerEnter2D()
	// Description:	Processes reactions to entering trigger regions on the map.
	//				Including: Doors/exits
	// Parameters:	Collider2D triggerZone	- The trigger region entered.
	protected void OnTriggerEnter2D (Collider2D triggerZone)
	{
		if (triggerZone.tag == "Lock")
		{
			Keyhole theLock = triggerZone.GetComponentInParent<Keyhole>();

			theLock.Unlock(this);
		}
	}
}
