/*
 * Filename:		FloorSwitch.cs
 * Programmer:		Colin McMillan
 * Date:			June 2015
 * Description:		Details the functionality and behaviours of a floor switch, triggered by 
 * 					characters standing on top of it.
 */


// Not currently in use/updated.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*	Name:		FloorSwitch
 *	Purpose:	A floor switch that can be triggered by placing objects on top of it.
 */
public class FloorSwitch : MonoBehaviour
{
	// The parent of the switch.
	public SwitchableObject parent;
	// The objects pressing down on this switch.
	List<Collider2D> pressedBy;
	// Whether this switch has weight on it or not.
	bool isWeighedDown;
	// Whether the weight on the switch has changed since the last frame.
	bool weightChanged;

	// A reference to the collision box used by the switch.
	Collider2D switchCollider;

	/*	Name:			Start()
	 * 	Description:	The initializations needed when this object is created.
	 */
	protected void Start()
	{
		pressedBy = new List<Collider2D>();
		isWeighedDown = false;
		weightChanged = false;

		switchCollider = GetComponent<Collider2D>();
	}



	/*	Name:			Update()
	 * 	Description:	The initializations needed when this object is created.
	 */
	protected void Update()
	{
		/* Because OnTriggerExit doesn't work for objects that get teleported
		 away from the switch, on each update the list is checked to make sure
		 any objects that are no longer on the switch aren't holding it down. */
		if (pressedBy.RemoveAll(item => !item.IsTouching(switchCollider)) > 0)
		{
			weightChanged = true;
		}


		if (weightChanged)
		{
			weightCheck();

			weightChanged = false;
		}
	}

	
	protected void OnTriggerEnter2D (Collider2D triggerObject)
	{
		pressedBy.Add(triggerObject);

		weightChanged = true;
	}



	protected void OnTriggerExit2D (Collider2D triggerObject)
	{
		pressedBy.Remove(triggerObject);

		weightChanged = true;
	}



	protected virtual void weightCheck ()
	{
		if (isWeighedDown && pressedBy.Count == 0)
		{
			steppedOff();
		}
		else if (!isWeighedDown && pressedBy.Count > 0)
		{
			steppedOn();
		}
	}



	private void steppedOn ()
	{
		parent.SwitchedOn();
		GetComponent<Renderer>().enabled = false;

		isWeighedDown = true;
	}



	private void steppedOff ()
	{
		parent.SwitchedOff();
		GetComponent<Renderer>().enabled = true;

		isWeighedDown = false;
	}
}
