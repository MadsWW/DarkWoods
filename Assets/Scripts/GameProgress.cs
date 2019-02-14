using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void OnGameWonEvent(SaveData data);

public class GameProgress : MonoBehaviour {

    public event OnGameWonEvent GameWonEvent;

    //Button Prefab + Parent Transform
    public GameObject ItemBut;
    public Transform invTransform;

    //Item For Win Condition
    private int _itemIDForWin = 12;
    public string ItemTakenText;

    private List<GameObject> inventory = new List<GameObject>();
    private Vector3 nextPos;
    private float buttonHeight;
    private Vector3 ogTransform;

    //Time Variables
    public Text TimeText;
    private int beginHour = 16;
    private int beginMin = 3;

    private int currentHour = 0;
    private int currentMinute = 0;

    private int endHour = 20;
    private int endMin = 0;

    private bool _hasTimeLeft = true;

    private void Start()
    {
        currentHour = beginHour;
        currentMinute = beginMin;
        TimeText.text = string.Format("{0}:{1}", GetCurrentHour(), GetCurrentMinute());
    }

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
                RemoveFromInventory(ItemButton.selectedButton);
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
                RemoveFromInventory(ItemButton.selectedButton);
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

    #region INGAME_TIME_METHODS

    public void AddTimeToTimer(int timePassed)
    {
        if (_hasTimeLeft)
        {
            currentMinute += timePassed;
            CalculateTimeToClockTime();
            HasTimeRemaining();

            string hourText = GetCurrentHour();
            string minuteText = GetCurrentMinute();

            TimeText.text = string.Format("{0}:{1}", hourText, minuteText);
            TimeText.color = Color.Lerp(Color.white, Color.black, CalculateColorTimeLerp());
        }
    }

    private string GetCurrentHour()
    {
        if (currentHour < 10) { return string.Format("0{0}", currentHour); }
        else{ return currentHour.ToString(); }
    }

    private string GetCurrentMinute() //TODO Can be one function pass min/hour as parameter
    {
        string timeInMin = "";

        if (currentMinute < 10) { timeInMin = string.Format("0{0}", currentMinute); return timeInMin; }
        else return currentMinute.ToString();
    }

    private void CalculateTimeToClockTime()
    {
        if (currentMinute >= 60)
        {
            int timeOverHour = currentMinute - 60;
            currentMinute = 0 + timeOverHour;
            currentHour++;
        }

        if (currentHour >= 24)
        {
            int hourOverHour = currentHour - 24;
            currentHour = 0 + hourOverHour;
        }
    }

    private float CalculateColorTimeLerp()
    {
        float hoursLeft = endHour - currentHour;
        float minutesLeft = endMin - currentMinute;
        float timeLeftInMinutes = (hoursLeft * 60) + minutesLeft;

        float beginHourLeft = endHour - beginHour;
        float beginMinLeft = endMin - beginMin;
        float timeLeftAtStart = (beginHourLeft * 60) + beginMinLeft;

        return (timeLeftInMinutes - timeLeftAtStart) / (0 - timeLeftAtStart);
    }



    #endregion //INGAME_TIME_METHODS

    #region WINLOSE_CONDITIONS

    private void CheckForWin(Item it)
    {
        if (it._itemID == _itemIDForWin)
        {
            if (_hasTimeLeft)
            {
                Invoke("Win", 3f);
            }
            else
            {
                Invoke("Lose", 3f);
            }
        }
    }

    private void Win()
    {
        SaveData data = new SaveData();
        data.GameWon = 1;
        GameWonEvent(data);
        SceneManager.LoadScene("WinScene");
        CancelInvoke("Win");
    }

    private void Lose()
    {
        SceneManager.LoadScene("LoseScene");
        CancelInvoke("Lose");
    }

    private void HasTimeRemaining()
    {
        float timeLeftHour = endHour - currentHour;
        float timeLeftMin = endMin - currentMinute;
        float timeLeft = (timeLeftHour * 60) + timeLeftMin;

        if (timeLeft < 0)
        {
            _hasTimeLeft = false;
        }
    }

    #endregion //WINLOSE_CONDITION

}
