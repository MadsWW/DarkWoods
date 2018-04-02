using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour {

    public GameObject ItemButton;
    public Transform invTransform;

    private List<Item> _inventory = new List<Item>();

    public int _itemIDForWin;

    private Vector3 butPos;

    public static Item selectedItem;
    public static Item mergeItem;
    public static Button selectedButton;
    public static Button mergeButton;
    // Make static Room currentRoom


    #region PUBLIC_FUNCTIONS

    // Checks if the player has got the required item to enter the next room.
    public bool CanEnterNextRoom (Item item, Room room)
	{
        bool noItemReqToEnter = room._requiredItemToEnter == null;
        bool gotItemReqToEnter = room._requiredItemToEnter == selectedItem;

        if (gotItemReqToEnter || noItemReqToEnter)
        {
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
        bool itemReq = room._requiredItemForItem == selectedItem;
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
    //WinCondition needs to be changed. ##
    private void CheckForWin (Item it)
	{
		if (it._itemID == _itemIDForWin) 
		{
			SceneManager.LoadScene("WinScene"); 
		}
	}

    //Add UI Button to Inventory UI when an item is picked up.
    private void AddInventoryButton(Item item)
    {
        GameObject go = Instantiate(ItemButton, invTransform) as GameObject;
        go.transform.position -= butPos;
        butPos += new Vector3(0, go.GetComponent<RectTransform>().rect.height * 0.75f, 0);
        ItemButton itemButton = go.GetComponent<ItemButton>();
        itemButton.SetButtonText(item);

    }

    #endregion PRIVATE_METHODS
}
