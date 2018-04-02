using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyController : MonoBehaviour
{

    public TextManager _textManager;
    public GameProgress _gameProgress;
    public WorldCreator _worldCreator;

    public Button[] _buttons;

    private int x = 0;
    private int y = 1;


    //Enter Next X Pos Room
    public void EnterNextXRoom(int nextx)
    {
        int roomNumber = _worldCreator._worldCoor[x + nextx, y] - 1;
        Room room = _textManager._rooms[roomNumber];
        Item itemNeeded = room._requiredItemToEnter;

        bool roomAvailable = room != null;

        if (roomAvailable)
        {
            if (_gameProgress.CanEnterNextRoom(itemNeeded, room))
            {
                x += nextx;
                _textManager.SetRoomText(room);
                SetDirectionButtons(room);
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
        int roomNumber = _worldCreator._worldCoor[x, y + nexty] - 1;
        Room room = _textManager._rooms[roomNumber];
        Item itemNeeded = room._requiredItemToEnter;

        bool roomAvailable = room != null;

        if (roomAvailable)
        {
            if (_gameProgress.CanEnterNextRoom(itemNeeded, room))
            {
                y += nexty;
                _textManager.SetRoomText(room);
                SetDirectionButtons(room);
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
        int roomNumber = _worldCreator._worldCoor[x, y] - 1;
        Room room = _textManager._rooms[roomNumber];
        _textManager.DescriptionText(room._roomDescription);
    }

    //Pick up item if possible.
    public void GrabItem()
    {
        Room room = _textManager._rooms[_worldCreator._worldCoor[x, y] - 1];
        Item itemNeeded = room._requiredItemForItem;
        Item item = room._item;

        if (_gameProgress.CanGrabItemInRoom(itemNeeded, room))
        {
            if (_gameProgress.ItemTakenFromRoom(room))
            {
                _textManager.DescriptionText(_textManager._pickUpText + item._itemName);
                GameProgress.selectedButton.GetComponent<Image>().color = Color.white;
                GameProgress.selectedButton = null;
                GameProgress.selectedItem = null;
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


    // Enables/Disables DirectionButtons depending on which way you can travel in current room.
    public void SetDirectionButtons(Room room)
    {
        foreach (Button but in _buttons)
        {
            but.interactable = false;
        }


        foreach (Direction dir in room._directions)
        {
            string direction = dir.ToString();
            switch (direction)
            {
                case "North":
                    _buttons[0].interactable = true;
                    break;
                case "East":
                    _buttons[1].interactable = true;
                    break;
                case "South":
                    _buttons[2].interactable = true;
                    break;
                case "West":
                    _buttons[3].interactable = true;
                    break;
            }
        }

    }

    // Merges the items into new item if possible.
    public void MergeItem()
    {
        // bool to check if the selectItems (from gamprogress) can be merged.
        // if can send message and let Gameprogress handle the merge.
        // if not send message that merge is not possible.
    }
}
