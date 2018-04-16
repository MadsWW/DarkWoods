using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour {

    //Button Prefab + Parent Transform
    public GameObject ItemBut;
    public Transform invTransform;

    //Item For Win Condition
    public int _itemIDForWin;
    public string ItemTakenText;

    private List<GameObject> inventory = new List<GameObject>();
    private Vector3 nextPos;
    private float buttonHeight;
    private Vector3 ogTransform;

    // initial health and deprectiation depending on good or bad move.
    public Slider healtSlider;
    private int health = 120;

    #region PUBLIC_FUNCTIONS

    // Checks if the player has got the required item to enter the next room.
    public bool CanEnterNextRoom (Item item, Room room)
	{
        bool noItemReqToEnter = room._requiredItemToEnter == null;
        bool gotItemReqToEnter = room._requiredItemToEnter == ItemButton.selectedItem;

        if (gotItemReqToEnter || noItemReqToEnter)
        {
            if (gotItemReqToEnter && room._requiredItemToEnter != null)
            {
                //RemoveFromInventory(ItemButton.selectedButton); //Enable if you want items removed when going in room with another item.
                room._requiredItemToEnter = null;
            }
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
            // Need Required Item to grab item and that item I need to grab must not be null.
            if (itemReq && room._requiredItemForItem != null)
            {
                //RemoveFromInventory(ItemButton.selectedButton); //Enable if you want items removed when grabbing item in room with another item.
                room._requiredItemForItem = null;  
            }
            //Check if item is not grabbed yet and there is an item in the room.
            if (!room._itemgrabbed && room._item != null)
            {
                room._itemgrabbed = true;
                room._roomDescription = ItemTakenText;
            }         
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
        if (ItemButton.selectedItem != null && ItemButton.mergeItem != null)
        {
            bool canMerge = ItemButton.selectedItem == ItemButton.mergeItem._mergeWithItem;
            if (canMerge)
            {
                RemoveFromInventory(ItemButton.selectedButton);
                RemoveFromInventory(ItemButton.mergeButton);
                TakeItem(ItemButton.selectedItem._mergeToItem);
                return true;
            }
        }
        return false;
    }
    
    #endregion PUBLIC_FUNCTIONS


    #region PRIVATE_INVENTORY_METHODS

    //Make object pooling when items count gets larger then screen can show. ##
    //Add UI Button to Inventory UI when an item is picked up.
    private void AddInventoryButton(Item item)
    {
        //Make new prefab and add to inventory;
        GameObject go = Instantiate(ItemBut, invTransform) as GameObject;
        ogTransform = go.transform.position;
        go.transform.position -= nextPos;
        inventory.Add(go);

        float height = go.GetComponent<RectTransform>().rect.height * 0.75f;
        Vector3 butHeight = new Vector3(0,height,0);

        SetButtonPosition(butHeight, go);

        //Sets Text of ButtonPrefab
        ItemButton itemButton = go.GetComponent<ItemButton>();
        itemButton.SetButtonText(item);
    }

    //(Re)sets position of the buttons from inventory.
    private void SetButtonPosition(Vector3 height, GameObject go)
    {
        nextPos = Vector3.zero;

        foreach (GameObject invButton in inventory)
        {
            invButton.transform.position = ogTransform;
            invButton.transform.position -= nextPos;
            nextPos += height;
        }
    }

    //Takes item from the room into inventory and deletes it from the room.
    private void TakeItem(Item item)
    {
        CheckForWin(item);
        AddInventoryButton(item);
    }

    private void RemoveFromInventory(Button button)
    {
        inventory.Remove(button.gameObject);
        Destroy(button.gameObject);
    }

    #endregion PRIVATE_INVENTORY_METHODS


#region PRIVATE_WINLOSECONDITION_METHODS

    public void ChangeHealth(int i)
    {
        health -= i;
        healtSlider.value = health;
        print(health + ": " + i);

        if (health <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
    }

    //Wins if a certain item is grabbed.
    //WinCondition needs to be changed. ##
    private void CheckForWin(Item it)
    {
        if (it._itemID == _itemIDForWin)
        {
            Invoke("Win", 3f);
        }
    }

    private void Win()
    {
        SceneManager.LoadScene("WinScene");
        CancelInvoke("Win");
    }

    #endregion PRIVATE_WINLOSECONDITION_METHODS

}
