/*
 * Filename:		SwitchableObject.cs
 * Programmer:		Colin McMillan
 * Date:			June 2015
 * Description:	
 */

// Not currently in use.

using UnityEngine;
using System.Collections;


// Name:	SwitchableObject
// Purpose:	Defines an object that is effected by switches. 
public class SwitchableObject : MonoBehaviour
{
	// If the switchable object is switched on
	public bool isSwitchedOn;
	/* If the switch is inverted - if it should be doing something
	when it's off, instead of when it's on. */
	public bool IsInverted;

	// The children of the switch.
	public SwitchableObject[] children;

	// Use this for initialization
	protected virtual void Start()
	{
		children = GetComponentsInChildren<SwitchableObject>();
	}
	
	// Update is called once per frame
	protected virtual void Update()
	{
		// Get the switch status, and invert it if necessary.
		bool switchStatus = isSwitchedOn;

		if (IsInverted)
		{
			switchStatus = !switchStatus;
		}


		// If the switch is turned on OR is turned off on an inverted switchable...
		if (switchStatus == true)
		{
			// Use the switched on behaviour.
			SwitchedOn();
		}
		// ... otherwise, use the switched off behaviour.
		else
		{
			SwitchedOff();
		}
	}



	// Name:		SwitchedOn()
	// Description:	The behaviour of this object when it is switched on.
	//				If the object was not previously switched on, it calls
	//				OnSwitchOn() to set the initial 
	public virtual void SwitchedOn()
	{
		if (!isSwitchedOn)
		{
			isSwitchedOn = true;
			OnSwitchOn();
		}
	}



	// Name:		OnSwitchOn()
	// Description:	What happens when an object is first switched on, including
	//				switching on all of its children.
	protected virtual void OnSwitchOn()
	{
		foreach (var child in children)
		{
			if ((child.tag == "Switchable") 
			    && (child.GetInstanceID() != this.GetInstanceID()))
			{
				child.SwitchedOn();
			}
		}
	}



	// Name:		SwitchedOff()
	// Description:	The behaviour of this object when it is switched off.
	//				If the object was not previously switched off, it calls
	//				OnSwitchOff() to set the initial 
	public virtual void SwitchedOff()
	{
		if (isSwitchedOn)
		{
			isSwitchedOn = false;
			OnSwitchOff();
		}
	}



	// Name:		OnSwitchOff()
	// Description:	What happens when an object is first switched off, including
	//				switching off all of its children.
	protected virtual void OnSwitchOff()
	{
		foreach (var child in children)
		{
			if ((child.tag == "Switchable") 
			    && (child.GetInstanceID() != this.GetInstanceID()))
			{
				child.SwitchedOff();
			}
		}
	}



	// Name:		ToggleSwitch()
	// Description:	Switches this object to the opposite state. If it was on,
	//				switch off. If it was off, switch on. Also toggles all children.
	public void ToggleSwitch()
	{
		isSwitchedOn = !isSwitchedOn;

		foreach (var child in children)
		{
			if ((child.tag == "Switchable") 
			    && (child.GetInstanceID() != this.GetInstanceID()))
			{
				child.ToggleSwitch();
			}
		}
	}
}
