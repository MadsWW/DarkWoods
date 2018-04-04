using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour {

    public GameObject ItemBut;
    public Transform invTransform;
    public int _itemIDForWin;

    private Vector3 butPosition;

    // Make Room currentRoom


    #region PUBLIC_FUNCTIONS

    // Checks if the player has got the required item to enter the next room.
    public bool CanEnterNextRoom (Item item, Room room)
	{
        bool noItemReqToEnter = room._requiredItemToEnter == null;
        bool gotItemReqToEnter = room._requiredItemToEnter == ItemButton.selectedItem;

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
        bool itemReq = room._requiredItemForItem == ItemButton.selectedItem;
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
            TakeItem(room._item);
            room._item = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanMerge()
    {
        bool canMerge = ItemButton.selectedItem == ItemButton.mergeItem._mergeWithItem;
        if (canMerge)
        {
            TakeItem(ItemButton.selectedItem._mergeToItem);
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
    private void TakeItem(Item item)
    {
        CheckForWin(item);
        AddInventoryButton(item);
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

    //Make object pooling when items count gets larger then screen can show. ##
    //Add UI Button to Inventory UI when an item is picked up.
    private void AddInventoryButton(Item item)
    {
        GameObject go = Instantiate(ItemBut, invTransform) as GameObject;
        go.transform.position -= butPosition;

        Vector3 buttonHeight = new Vector3(0, go.GetComponent<RectTransform>().rect.height * 0.75f, 0);
        butPosition += buttonHeight;

        ItemButton itemButton = go.GetComponent<ItemButton>();
        itemButton.SetButtonText(item);

    }

    #endregion PRIVATE_METHODS
}
