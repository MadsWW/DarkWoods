using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyController : MonoBehaviour {

	public TextManager _textManager;
	public GameProgress _gameProgress;
	public WorldCreator _worldCreator;

	private int x = 0;
	private int y = 1;


    //Enter Next X Pos Room
    public void EnterNextXRoom(int nextx)
    {
        Room room = _textManager._rooms[_worldCreator._worldCoor[x + nextx, y] - 1];
        Item itemNeeded = room._requiredItemToEnter;

        bool roomAvailable = room != null;

        if (roomAvailable)
        {
            if (_gameProgress.CanEnterNextRoom(itemNeeded,room))
            {
                x += nextx;
                _textManager.SetRoomText(room);
            }
            else
            {
                _textManager.DescriptionText(_textManager._cantEnterRoom);
            }
        }
    }

    //Enter Next Y Pos Room
    public void EnterNextYRoom(int nexty)
    {
        Room room = _textManager._rooms[_worldCreator._worldCoor[x, y + nexty] - 1];
        Item itemNeeded = room._requiredItemToEnter;

        bool roomAvailable = room != null;

        if (roomAvailable)
        {
            if (_gameProgress.CanEnterNextRoom(itemNeeded,room)) 
            {
                y += nexty;
                _textManager.SetRoomText(room);
            }
            else
            {
                _textManager.DescriptionText(_textManager._cantEnterRoom);
            }
        }
    }

    //Sets Description Text of current Room
    public void InspectRoom()
    {
        int roomNumber = _worldCreator._worldCoor[x, y]-1;
        _textManager.SetDescriptionText(roomNumber);
    }

    //Pick up item if possible.
    public void GrabItem()
    {
        Room room = _textManager._rooms[_worldCreator._worldCoor[x, y]-1];
        Item itemNeeded = room._requiredItemForItem;
        Item item = room._item;
        
        if(_gameProgress.CanGrabItemInRoom(itemNeeded, room))
        {
            if (_gameProgress.ItemTakenFromRoom(room))
            {
                _textManager.DescriptionText(_textManager._pickUpText + item._itemName);
            }
            else
            {
                _textManager.DescriptionText(_textManager._noItemText);
            }
        }
        else
        {
            _textManager.DescriptionText(_textManager._cantPickupItem);
        }
    }
}
