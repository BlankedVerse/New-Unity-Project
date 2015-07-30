/*
* Filename:		FloorSwitch.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public enum TileType
{
	FIELD = 0,
	WALL,
	PLAYERSTART
}


/* Name:	WildernessSpawner
 * Purpose:	Spawns a randomized map, based on predetermined size limits and
 * 			tile type probabilities.
 */
public class WildernessSpawner : MonoBehaviour
{
	
	const int startBuffer = 2;


	// A short-hand of the map
	private List<List<TileType>> tilemap;
	public TileType [,] tileList;

	// The minimum width of the map, in tiles.
	public int MinWidth;
	// The maximum width of the map, in tiles.
	public int MaxWidth;
	// The minimum height of the map, in tiles.
	public int MinHeight;
	// The minimum height of the map, in tiles.
	public int MaxHeight;


	public int PartsField;
	public int PartsWall;


	public GameObject playerPrefab;
	public GameObject floorPrefab;
	public GameObject wallPrefab;


	// The actual width of the map, in tiles.
	public int width;
	// The actual height of the map, in tiles.
	public int height;

	// Use this for initialization
	void Start ()
	{
		tilemap = new List<List<TileType>>();

		width = Random.Range(MinWidth, MaxWidth);
		height = Random.Range(MinHeight, MaxHeight);


		// For each column on the tilemap...
		for (int x = 0; x < width; x++)
		{
			// Create a row.
			List<TileType> currentLine = new List<TileType>();

			/* If the line doesn't represent the eastern or western edge 
			 * of the map... */
			if ((x > 0) && (x < (width - 1)))
			{
				// Add the north-most boundary.
				currentLine.Add(TileType.WALL);

				// Fill the intervening space with random spaces
				for (int y = 1; y < (height - 1); y++)
				{
					currentLine.Add(randomTile());
				}

				// Add the south-most boundary.
				currentLine.Add(TileType.WALL);
			}
			// For the east/west edges...
			else
			{
				// Fill the line with wall tiles.
				for (int y = 0; y < height; y++)
				{
					currentLine.Add(TileType.WALL);
				}
			}

			// Add the completed line to the tilemap
			tilemap.Add(currentLine);
		}

		// Assign the player starting location
		setPlayerStart();


		InstantiateMap();
	}



	/*	Name:			randomTile()
	 *  Description:	Returns a random tile ID based on the declared probabilities
	 * 					of various tiles appearing.
	 *	Parameters:		
	 *	Returns:		A randomly determined TileType
	 */
	private TileType randomTile()
	{
		int total = PartsWall + PartsField;

		int randomRoll = Random.Range(0, total);

		// Default tiles to a FIELD space
		TileType returnTile = TileType.FIELD;


		// If the roll is under the "parts wall" number, it's a wall.
		if (randomRoll < PartsWall)
		{
			returnTile = TileType.WALL;
		}
		// Otherwise, the tile remains a field.

		return returnTile;
	}



	/*	Name:			setPlayerStart()
	 *  Description:	Randomly sets the player's starting location.
	 */
	private void setPlayerStart()
	{
		int playerX = Random.Range(startBuffer, width - startBuffer);
		int playerY = Random.Range(startBuffer, height - startBuffer);



		tilemap[playerX][playerY] = TileType.PLAYERSTART;
	}


	
	/*	Name:			InstantiateMap()
	 *  Description:	Instantiates and adds as children all the tiles and
	 * 					objects for this map.
	 */
	public void InstantiateMap()
	{
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				GameObject newTile = null;

				switch (tilemap[x][y])
				{
				case TileType.FIELD:
					newTile = floorPrefab;
					break;
				case TileType.WALL:
					newTile = wallPrefab;
					break;
				case TileType.PLAYERSTART:
					newTile = floorPrefab;

					GameObject player = playerPrefab;

					player =
						Instantiate(player, new Vector3(x, y, 0f), Quaternion.identity)
						as GameObject;

					player.transform.SetParent(this.transform);
					break;
				default:
					newTile = floorPrefab;
					break;
				}

				newTile = 
					Instantiate(newTile, new Vector3(x, y, 0f), Quaternion.identity)
					as GameObject;

				newTile.transform.SetParent(this.transform);
			}
		}
	}
}
