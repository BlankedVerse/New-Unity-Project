using UnityEngine;
using System.Collections;

public class SwitchableDoor : SwitchableObject
{
	// Name:		OnSwitchOn()
	// Description:	What happens when an object is first switched on, including
	//				switching on all of its children.
	protected override void OnSwitchOn()
	{
		gameObject.SetActive(false);
	}
	
	
	
	// Name:		OnSwitchOff()
	// Description:	What happens when an object is first switched off, including
	//				switching on all of its children.
	protected override void OnSwitchOff()
	{
		gameObject.SetActive(true);
	}
}
