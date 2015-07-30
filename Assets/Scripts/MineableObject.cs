/*
* Filename:		MineableObject.cs
* Programmer:	Colin McMillan
* Date:			July 2015
* Description:	An object that can be mined, spawning pickups and degrading when hit.
*/

using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;      // Tells Random to use the Unity Engine random number generator.


// Name:	MineableObject
// Purpose:	Describes the behaviours and properties of a mineable object.
public class MineableObject : MonoBehaviour
{
	// How many "hits" it takes to destroy a mineable rock.
	public int HitsPerLevel;
	// How many hits the mineable resource has taken this stage.
	private int hitsTaken;
	// The stage the mineable resource is at - how many more times can it be mined?
	private int currentStage;

	// A reference to the mine's sprite renderer.
	private SpriteRenderer renderer;
	// A list of sprites the mine uses as it is used up.
	public Sprite [] SpriteList;

	// The common resources spawned by the mine.
	public GameObject [] CommonResourcePrefabs;
	// The rare resources spawned by the mine.
	public GameObject [] RareResourcePrefabs;
	// The percentage chance of mining a rare resource.
	public int RareSpawnPercentage;

	// Use this for initialization
	void Start ()
	{
		currentStage = 0;

		renderer = GetComponent<SpriteRenderer>();
	}
	


	// Name:		OnMine()
	// Description:	Mines this source, spawning resources and reducing the
	//				mine's health.
	public void OnMine(int mineStrength)
	{
		// Take the hits.
		hitsTaken += mineStrength;

		// If the hits would put the mine into another level...
		if (hitsTaken >= HitsPerLevel)
		{
			// Reset the hits, and increment the stage.
			HitsPerLevel = 0;
			hitsTaken = 0;

			currentStage += 1;

			// If the mine has run out of stages...
			if (currentStage >= SpriteList.Length)
			{
				// Kill the mine!
			}
			else
			{
				// Change the sprite and spawn a resource.
				renderer.sprite = SpriteList[currentStage];
			}
		}
	}
}
