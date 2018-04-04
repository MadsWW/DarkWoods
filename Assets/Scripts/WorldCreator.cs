using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Vector2 should be added to RoomXML data.
// Makes for easier extension of game. - Less hardcoded data. 
public class WorldCreator : MonoBehaviour {

	public int[,] _worldCoor = new int [5,4];

	// Use this for initialization
	void Start () 
	{
		_worldCoor[0,1] = 1;
		_worldCoor[1,0] = 2;
		_worldCoor[1,1] = 3;
		_worldCoor[2,1] = 4;
		_worldCoor[2,2] = 5;
		_worldCoor[2,3] = 6;
		_worldCoor[3,0] = 7;
		_worldCoor[3,1] = 8;
		_worldCoor[3,3] = 9;
		_worldCoor[4,1] = 10;
		_worldCoor[4,2] = 11;
		_worldCoor[4,3] = 12;
	}
}
