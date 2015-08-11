/*
 * Filename:		WildernessSpawner.cs
 * Programmer:		Colin McMillan
 * Date:			August 2015
 * Description:		Defines the WildernessSpawner script, which creates a randomized
 * 					map for the player to explore. The game object this is attached to will
 * 					be the parent to all tiles and objects created.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;


/* Name:	TileType
 * Purpose:	Lists the types of tiles that can be placed on a map.
 */
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
	// The minimum distance between the player's start location and the edge of the map.
	const int startBuffer = 2;


	// A short-hand version of the map.
	private List<List<TileType>> tilemap;

	//public TileType [,] tileList;

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


	/*	Name:			Start()
	 * 	Description:	The initializations needed when this object is created.
	 * 					Creates a map die and a new map based on the values input into
	 * 					the editor.
	 */
	void Start ()
	{
		// Create a die to roll for tile types.
		mapDie = new List<TileType>();

		/* For each type of tile, add a number of sides to the die in
		 * proportion to the ratio set in the editor. */
		mapDie.AddRange(Enumerable.Repeat(TileType.FIELD, PartsField));
		mapDie.AddRange(Enumerable.Repeat(TileType.WALL, PartsWall));
		mapDie.AddRange(Enumerable.Repeat(TileType.MINE, PartsMine));


		// Create and instantiate a new map.
		CreateMap();
	}



	/*	Name:			CreateMap()
	 *	Description:	Randomizes and instantiates a new map.
	 */
	public void CreateMap()
	{
		// Set the map to a random size with random tiles.
		randomizeMap();
		
		// Assign the player starting location.
		setPlayerStart();
		
		// Instantiate the tiles and objects of the map.
		instantiateMap();
	}



	/*	Name:			randomizeMap()
	 * 	Description:	Randomizes the size of the map and creates a new
	 * 					tilemap to be instantiated.
	 */
	private void randomizeMap()
	{
		// Initialize/reinitialize the tilemap list.
		tilemap = new List<List<TileType>>();

		// Set the width and height of the map.
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
	}



	/*	Name:			randomTile()
	 *  Description:	Returns a random tile ID based on the declared probabilities
	 * 					of various tiles appearing.
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
		// The player's starting X position in the tilemap.
		int playerX = Random.Range(startBuffer, width - startBuffer);
		// The player's starting Y position in the tilemap.
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



	/*	Name:			placePlayer()
	 *  Description:	Instatiates a player and warp pad at the given coordinates.
	 * 	Parameters:		int x	- The x coordinate to place the player.
	 * 					int y	- The y coordinate to place the player.
	 */
	private void placePlayer(int x, int y)
	{
		// Get the player prefab...
		GameObject player = PlayerPrefab;
		// Get the warpPad prefab...
		GameObject warpPad = PadPrefab;

		
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
	}



	/*	Name:			placeMine()
	 *  Description:	Instatiates a mine at the given coordinates.
	 * 	Parameters:		int x	- The x coordinate to place the mine.
	 * 					int y	- The y coordinate to place the mine.
	 */
	private void placeMine(int x, int y)
	{
		// Get the mine prefab...
		GameObject newMine = MinePrefab;

		// Instantiate a mine at this location...
		newMine =
			Instantiate(newMine, new Vector3(x, y, 0f), Quaternion.identity)
				as GameObject;
		
		// And set the mine as a child of the wilderness spawner object.
		newMine.transform.SetParent(this.transform);
	}


	
	/*	Name:			instantiateMap()
	 *  Description:	Instantiates and adds as children all the tiles and
	 * 					objects for this map.
	 */
	private void instantiateMap()
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
					// Instantiate a mine...
					placeMine(x, y);
					// Put some empty floor under it.
					newTile = FloorPrefab;
					break;
				case TileType.PLAYERSTART:
					// Instantiate the player and warp pad...
					placePlayer(x, y);
					// Put some empty floor under it.
					newTile = FloorPrefab;
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
