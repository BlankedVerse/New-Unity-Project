/*
 * Filename:		FloorTile.cs
 * Programmer:		Colin McMillan
 * Date:			July 2015
 * Description:		A floor tile, which randomly assigns itself one of several sprites on creation.
 */

using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;      // Tells Random to use the Unity Engine random number generator.


/* 	Name:		FloorTile
 * 	Purpose:	Describes a floor tile, which randomly chooses a sprite from its list.
 */
public class FloorTile : MonoBehaviour
{
	// The sprites used by this floor tile.
	public Sprite [] SpriteList;

	/*	Name:			Start()
	 * 	Description:	The initializations needed when this object is created.
	 * 					It randomizes the sprite used based on sprites entered in the editor.
	 */
	void Start ()
	{
		if (SpriteList.Length > 0)
		{
			// Set the floor tile's sprite to be a random sprite from its list.
			GetComponent<SpriteRenderer>().sprite = SpriteList[Random.Range(0, SpriteList.Length)];
		}
	}
}
