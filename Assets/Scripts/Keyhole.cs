/*
* Filename:		Keyhole.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/
		
		
using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour
{
	protected bool isOpen;

	protected virtual void Start()
	{
		isOpen = false;
	}


	// Name:		OnUnlock()
	// Description:	The behaviours that happen when this keyhole is opened
	public virtual bool Unlock(Key keyToUse)
	{
		isOpen = true;

		foreach (Transform child in transform)
		{
			if (child.tag == "Lock")
			{
				child.gameObject.SetActive(false);
			}
		}

		// Use up the key.
		keyToUse.UseKey();

		return true;
	}
}
