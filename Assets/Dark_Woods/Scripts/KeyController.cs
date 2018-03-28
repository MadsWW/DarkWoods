using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyController : MonoBehaviour {

	public TextManager _textManager;
	public GameProgress _gameProgress;
	public WorldCreator _worldCreator;

	private int x = 0;
	private int y = 1;

	void Update () 
	{

		// All arrowkey input checks if possible to go to next room and goes there if so.
		if (Input.GetKeyUp (KeyCode.UpArrow) && _worldCreator._worldCoor [x + 1, y] != 0) 
		{
			if (_gameProgress.CanEnterNextRoom (_worldCreator._worldCoor [x + 1, y])) 
			{
				x++;
				_textManager.SetRoomText (_worldCreator._worldCoor [x, y]-1);
			}
		}

		if (Input.GetKeyUp (KeyCode.DownArrow) && _worldCreator._worldCoor [x - 1, y] != 0) 
		{
			if (_gameProgress.CanEnterNextRoom (_worldCreator._worldCoor [x - 1, y])) 
			{
				x--;
				_textManager.SetRoomText (_worldCreator._worldCoor [x, y]-1);
			}
		}

		if (Input.GetKeyUp (KeyCode.LeftArrow) && _worldCreator._worldCoor [x, y - 1] != 0) 
		{
			if(_gameProgress.CanEnterNextRoom(_worldCreator._worldCoor [x, y - 1]))
			{
				y--;
				_textManager.SetRoomText (_worldCreator._worldCoor [x, y]-1);
			}
		}

		if (Input.GetKeyUp (KeyCode.RightArrow) && _worldCreator._worldCoor [x, y + 1] != 0) 
		{
			if(_gameProgress.CanEnterNextRoom (_worldCreator._worldCoor [x, y + 1]))
			{
				y++;
				_textManager.SetRoomText (_worldCreator._worldCoor [x, y]-1);
			}
		}

		//Sets text of current room.
		if (Input.GetKeyDown (KeyCode.L)) 
		{
			_textManager.SetDescriptionText (_worldCreator._worldCoor [x, y] - 1);
		}

		//Pick up item if possible.
		if (Input.GetKeyDown (KeyCode.T)) 
		{
			if (_gameProgress.CanGrabItemInRoom (_worldCreator._worldCoor [x, y]-1))
			{
				_gameProgress.TakeItemToInventory (_worldCreator._worldCoor [x, y]-1);
			} 
		}
	}
}
