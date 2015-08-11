/*
 * Filename:		ResourcePickup.cs
 * Programmer:		Colin McMillan
 * Date:			August 2015
 * Description:		Describes the actions and behaviours of the warp pad.
 */


using UnityEngine;
using System.Collections;


// Name:		ResourcePickup
// Purpose:		The behaviours and 
public class ResourcePickup : LiftableObject
{
	// The value of the resource
	public int value;


	/* 	Name:			Value()
	 * 	Description:	Returns the value of this pickup.
	 * 	Returns:		The value of the pickup.
	 */
	public int Value()
	{
		return value;
	}


	/*
	// Name:		OnTriggerEnter2D()
	// Description:	Processes reactions to entering trigger regions on the map.
	// Parameters:	Collider2D trigger	- The trigger region entered.
	protected void OnTriggerEnter2D (Collider2D trigger)
	{
		//base.OnTriggerEnter2D(triggerZone);
		
		// Check for an exit
		if (trigger.tag == "WarpPad")
		{
			//ResourcePickup resourceToWarp = trigger.GetComponent<ResourcePickup>();
			
			Player.AcquireMineral(value);
			
			Destroy(this.gameObject);
		}
	}*/
}