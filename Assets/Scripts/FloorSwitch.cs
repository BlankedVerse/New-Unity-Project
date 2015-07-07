/*
* Filename:		FloorSwitch.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Name:	FloorSwitch
// Purpose:	
public class FloorSwitch : MonoBehaviour
{
	public SwitchableObject parent;
	List<Collider2D> pressedBy;
	bool isWeighedDown;
	bool weightChanged;

	Collider2D switchCollider;

	protected void Start ()
	{
		pressedBy = new List<Collider2D>();
		isWeighedDown = false;
		weightChanged = false;

		switchCollider = GetComponent<Collider2D>();
	}



	protected void Update ()
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
