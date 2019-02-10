using UnityEngine;
using UnityEngine.UI;


public class KeyController : MonoBehaviour
{
    //TODO Make Custom Events for each separate button to make more Methods private. 
    //TODO Combine the EnterNextRoom Functions.


    [Header("Holds Scripts named:")]
    public TextManager _textManager;
    public GameProgress _gameProgress;
    public WorldCreator _worldCreator;

    [Header("Holds navButtons")]
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
                ItemButton.ResetButton();
            }
            else
            {
                _textManager.AddActionToQueue(_textManager._cantEnterRoom);
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
                ItemButton.ResetButton();
            }
            else
            {
                _textManager.AddActionToQueue(_textManager._cantEnterRoom);
            }
        }
    }

    //Sets Description Text of current Room
    public void InspectRoom()
    {
        if (!_gameProgress.TimeText.enabled)
        {
            _gameProgress.TimeText.enabled = true;
        }

        int roomNumber = _worldCreator._worldCoor[x, y] - 1;
        Room room = _textManager._rooms[roomNumber];
        _textManager.AddActionToQueue(room._roomDescription);
    }

    //Sets text depending if grabitem is possible.
    public void GrabItem()
    {
        EnableNavButtons();

        Room room = _textManager._rooms[_worldCreator._worldCoor[x, y] - 1];
        Item itemNeeded = room._requiredItemForItem;
        Item item = room._item;

        if (_gameProgress.CanGrabItemInRoom(itemNeeded, room))
        {
            if (_gameProgress.ItemTakenFromRoom(room))
            {
                _textManager.AddActionToQueue(_textManager._pickUpText + item._itemName);
                ItemButton.ResetButton();
            }
            else
            {
                _textManager.AddActionToQueue(_textManager._noItemText);
            }
        }
        else
        {
            _textManager.AddActionToQueue(_textManager._cantPickupItem);
        }
    }

    private void EnableNavButtons()
    {
        if (_buttons[0].gameObject.activeSelf) { return; }
        else
        {
            foreach(Button button in _buttons)
            {
                button.gameObject.SetActive(true);
            }
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

    // Sets text if merge of items is succesfull or not.
    public void MergeItem()
    {
        if (_gameProgress.CanMerge())
        {
            _textManager.AddActionToQueue("Merge Succesfull");
            ItemButton.ResetButton();
        }
        else
        {
            _textManager.AddActionToQueue("Select Items that are capable of merging.");
        }
    }
}
