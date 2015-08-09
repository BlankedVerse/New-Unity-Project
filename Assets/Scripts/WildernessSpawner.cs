/*
* Filename:		FloorSwitch.cs
* Programmer:	Colin McMillan
* Date:			June 2015
* Description:	
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;


public enum TileType
{
	FIELD = 0,
	WALL,
	PLAYERSTART,
	MINE
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
	public int PartsMine;


	public GameObject PlayerPrefab;
	public GameObject FloorPrefab;
	public GameObject WallPrefab;
	public GameObject PadPrefab;
	public GameObject MinePrefab;


	// The actual width of the map, in tiles.
	public int width;
	// The actual height of the map, in tiles.
	public int height;


	private List<TileType> mapDie;

	// Use this for initialization
	void Start ()
	{
		tilemap = new List<List<TileType>>();




		mapDie = new List<TileType>();

		mapDie.AddRange(Enumerable.Repeat(TileType.FIELD, PartsField));
		mapDie.AddRange(Enumerable.Repeat(TileType.WALL, PartsWall));
		mapDie.AddRange(Enumerable.Repeat(TileType.MINE, PartsMine));



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
		int randomRoll = Random.Range(0, mapDie.Count);

		// Default tiles to a FIELD space
		TileType returnTile = mapDie[randomRoll];


		return returnTile;
	}



	/*	Name:			setPlayerStart()
	 *  Description:	Randomly sets the player's starting location.
	 */
	private void setPlayerStart()
	{
		int playerX = Random.Range(startBuffer, width - startBuffer);
		int playerY = Random.Range(startBuffer, height - startBuffer);


		/* The startBuffer guarantees that the spaces around the player are
		 accessible indices of the tilemap. (i.e. +-1 isn't out of range.)*/

		// For each space around the player's start position...
		for (int x = playerX - 1; x <= playerX + 1; x++)
		{
			for (int y = playerY - 1; y <= playerY + 1; y++)
			{
				// Set the tile to be a field tile.
				tilemap[x][y] = TileType.FIELD;
			}
		}
		

		// Set the player's start location.
		tilemap[playerX][playerY] = TileType.PLAYERSTART;
	}


	
	/*	Name:			InstantiateMap()
	 *  Description:	Instantiates and adds as children all the tiles and
	 * 					objects for this map.
	 */
	public void InstantiateMap()
	{
		// For each point on the map...
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				GameObject newTile = null;

				// Depending on the TileType at that point, set the tile prefab to instantiate.
				switch (tilemap[x][y])
				{
				case TileType.FIELD:
					newTile = FloorPrefab;
					break;
				case TileType.WALL:
					newTile = WallPrefab;
					break;
				case TileType.MINE:
					// Get the mine prefab...
					GameObject newMine = MinePrefab;

					// Put some empty floor under it.
					newTile = FloorPrefab;
					
					// Instantiate a mine at this location...
					newMine =
						Instantiate(newMine, new Vector3(x, y, 0f), Quaternion.identity)
							as GameObject;
					
					// And set the mine as a child of the wilderness spawner object.
					newMine.transform.SetParent(this.transform);
					break;
				case TileType.PLAYERSTART:
					// Get the player prefab...
					GameObject player = PlayerPrefab;
					// Get the warpPad prefab...
					GameObject warpPad = PadPrefab;

					// Put some empty floor under it.
					newTile = FloorPrefab;

					// Instantiate a player and a warp pad at this location...
					player =
						Instantiate(player, new Vector3(x, y, 0f), Quaternion.identity)
							as GameObject;
					warpPad =
						Instantiate(warpPad, new Vector3(x, y, 0f), Quaternion.identity)
							as GameObject;

					// And set the player and warp pad as children of the wilderness spawner object.
					player.transform.SetParent(this.transform);
					warpPad.transform.SetParent(this.transform);
					break;
				
				// When in doubt, make it a floor tile.
				default:
					newTile = FloorPrefab;
					break;
				}

				// Instatiate the selected tile prefab...
				newTile = 
					Instantiate(newTile, new Vector3(x, y, 0f), Quaternion.identity)
						as GameObject;

				// And set this tile as a child of the wilderness spawner object.
				newTile.transform.SetParent(this.transform);
			}
		}
	}
}
