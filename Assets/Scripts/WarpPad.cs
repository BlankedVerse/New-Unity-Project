/*
 * Filename:		WarpPad.cs
 * Programmer:		Colin McMillan
 * Date:			August 2015
 * Description:		Describes the actions and behaviours of the warp pad.
 */


using UnityEngine;
using System.Collections;


/* Name:	WarpPad
 * Purpose:	The behaviours of a warp pad, which can retrieve the player's dropped-off resources.
 */
public class WarpPad : MonoBehaviour
{
	/* 	Name:			OnTriggerEnter2D()
	 * 	Description:	Processes reactions to entering trigger regions on the map.
	 *					Including: Doors/exits
	 * 	Parameters:		Collider2D trigger	- The trigger region entered.
	 */
	protected void OnTriggerEnter2D (Collider2D trigger)
	{
		//base.OnTriggerEnter2D(triggerZone);
		
		// Check for any resource dropped on the WarpPad.
		if (trigger.tag == "Resource")
		{
			ResourcePickup resourceToWarp = trigger.GetComponent<ResourcePickup>();

			Player.AcquireMineral(resourceToWarp.Value());

			Destroy(trigger.gameObject);

			//this.GetComponent<Animator>().
		}
	}
}