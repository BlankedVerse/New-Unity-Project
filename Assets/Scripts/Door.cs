/*
* Filename:		Door.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/


using UnityEngine;
using System.Collections;


// Name:	Door
// Purpose:	
public class Door : MonoBehaviour
{
	// The direction to move through the door
	public Direction exitThrough;

	// Name:		Start()
	// Description:	Use this for initialization
	void Start ()
	{
		// The rotation of the door.
		int rotation = Mathf.FloorToInt(transform.eulerAngles.z);


		// Use the Z rotation to set the exit direction.
		switch (rotation)
		{
		case 0:
		case 360:
		case -360:
			exitThrough = Direction.NORTH;
			break;
		case 180:
		case -180:
			exitThrough = Direction.SOUTH;
			break;
		case -270:
		case 90:
			exitThrough = Direction.WEST;
			break;
		case -90:
		case 270:
			exitThrough = Direction.EAST;
			break;
		// If an invalid direction is used, destroy this! Can't handle it.
		default:
			Destroy(gameObject);
			break;
		}
	}
	
	// Name:		Update()
	// Description:	Update is called once per frame
	void Update ()
	{
	
	}
}
