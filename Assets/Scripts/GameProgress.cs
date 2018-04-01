using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour {

    public GameObject ItemButton;
    public Transform invTransform;

	private List<Item> _inventory = new List<Item>();

    public List<Button> _buttons;

	public int _itemIDForWin;

    private Vector3 butPos;


    #region PUBLIC_FUNCTIONS

    // Checks if the player has got the required item to enter the next room.
    public bool CanEnterNextRoom (Item item, Room room)
	{
        bool noItemReqToEnter = room._requiredItemToEnter == null;
        bool gotItemReqToEnter = CheckInInventory(item);

        if (gotItemReqToEnter || noItemReqToEnter)
        {
            SetDirectionButtons(room);
            return true;
        }
        else
        {
            return false;
        }
	}

	//Checks if player has got required item in inventory to grab the item in the room.
	public bool CanGrabItemInRoom (Item item,Room room)
	{
        bool itemReq = CheckInInventory(item);
        bool noItemReq = room._requiredItemForItem == null;

        if (itemReq || noItemReq) 
	    {
            return true;
        }
        else
        {
            return false;
        }

	}

    //Checks if there is an item in the room.
    public bool ItemTakenFromRoom(Room room)
    {
        if (room._item != null)
        {
            TakeItem(room);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    #endregion PUBLIC_FUNCTIONS


    // Enables/Disables DirectionButtons depending on which way you can travel in current room.
    public void SetDirectionButtons(Room room)
    {
        foreach(Button but in _buttons)
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

    #region PRIVATE_METHODS

    //Takes item from the room into inventory and deletes it from the room.
    private void TakeItem(Room room)
    {
        Item item = room._item;
        _inventory.Add(item);
        CheckForWin(item);
        AddInventoryButton(item);
        room._item = null;
    }

    //Wins if a certain item is grabbed.
    // Need to rework this later on!!
    private void CheckForWin (Item it)
	{
		if (it._itemID == _itemIDForWin) 
		{
			SceneManager.LoadScene("WinScene"); 
		}
	}

    //Checks if parameter item is in inventory.
    private bool CheckInInventory(Item it)
    {
        if(_inventory != null)
        {
            foreach (Item item in _inventory)
            {
                if(item == it)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Add UI Button to Inventory UI when an item is picked up.
    private void AddInventoryButton(Item item)
    {
        GameObject go = Instantiate(ItemButton, invTransform) as GameObject;
        go.transform.position -= butPos;
        butPos += new Vector3(0, go.GetComponent<RectTransform>().rect.height * 0.75f, 0);
        Text buttonText = go.GetComponentInChildren<Text>(); ;
        buttonText.text = item._itemName;
    }

    #endregion PRIVATE_METHODS
}
