/*
 * Filename:		GameManager.cs
 * Programmer:	Colin McMillan
 * Date:			June 2015
 * Description:	
 */


using UnityEngine;
using System.Collections;


/*  Name:		GameManager
 *  Purpose:	A singleton class that defines and manages the basic structure of gameflow.
 */
public class GameManager : MonoBehaviour
{
	public static GameManager manager = null;


	/*	Name:			Awake()
	 * 	Description:	The initializations needed when this object is created.
	 * 					Called earlier than Start(), it ensures that only one GameManager can exist.
	 */
	void Awake()
	{
		// Enforce singleton behaviour on the GameManager class
		// If there's no GameManager, make it this one
		if (manager == null)
		{
			manager = this;
		}
		// If there's a GameManager other than this, delete this
		else if (manager != this)
		{
			Destroy(gameObject);
		}

		// Don't destroy the map manager on reloading a scene.
		DontDestroyOnLoad(gameObject);


		// Load up the starting room.
	}
	


	/*	Name:			Update()
	 * 	Description:	Behaviours that happen for this object each frame.
	 */
	void Update ()
	{
	
	}
}
