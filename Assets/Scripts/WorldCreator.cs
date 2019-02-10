using UnityEngine;

 //TODO Get the size from the roomdata and set the coordinates value accordingly.

public class WorldCreator : MonoBehaviour {

	public int[,] _worldCoor = new int [5,5];

	// Use this for initialization
	void Start () 
	{
		_worldCoor[0,1] = 1;
        _worldCoor[0,3] = 2;
		_worldCoor[1,0] = 3;
		_worldCoor[1,1] = 4;
        _worldCoor[1,3] = 5;
		_worldCoor[2,1] = 6;
		_worldCoor[2,2] = 7;
		_worldCoor[2,3] = 8;
        _worldCoor[2,4] = 9;
		_worldCoor[3,0] = 10;
		_worldCoor[3,1] = 11;
		_worldCoor[3,3] = 12;
		_worldCoor[4,1] = 13;
		_worldCoor[4,2] = 14;
		_worldCoor[4,3] = 15;
        _worldCoor[4,4] = 16;
	}
}
