using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;


public enum Direction { North, East, South, West };

public class TextManager : MonoBehaviour {

    KeyController keyControl;

    [Header("All UIText Elements")]
    public Text roomName;
	public Text roomText;
	public Text itemDescription;

	public List<Room> _rooms = new List<Room>(); // Make property so only get is accesible
	public List<Item> _items = new List<Item>(); // Make property so only get is accesible

    //Holds latest x action performed by user
    public Queue<string> performedAction = new Queue<string>();
    int count = 0;
    int maxCount = 3;

    [Header("XML Documents")]
	public TextAsset xmlRoom;
	public TextAsset xmlItem;

    [Header("Various standard strings")]
	public string _noItemText;
    public string _cantEnterRoom;
    public string _cantPickupItem;
    public string _pickUpText;

    public int resetDescriptionTimer;

    // Sets all text for first room
	private void Start ()
	{
		FillRoomItemList();
		SetRoomText (_rooms[0]);
		ResetDescriptionText ();

        keyControl = FindObjectOfType<KeyController>();
        Room room = _rooms[0];
        keyControl.SetDirectionButtons(room);
    }

    #region PRIVATE_FUNCTIONS

    //Takes all XML data in turns into Lists filled with Room/Item Classes.
    private void FillRoomItemList ()
	{
		Item item;
		Room room;

		XmlDocument roomData = new XmlDocument();
		XmlDocument itemData = new XmlDocument();

        //XML Files
		roomData.LoadXml(xmlRoom.text);
		itemData.LoadXml(xmlItem.text);

        // Takes data from XMLRoom to individual lists.
        XmlNodeList roomIDList = roomData.GetElementsByTagName("RoomID");
        XmlNodeList roomNameList = roomData.GetElementsByTagName ("RoomName");
		XmlNodeList roomTextList = roomData.GetElementsByTagName ("RoomText");
		XmlNodeList roomDescriptionList = roomData.GetElementsByTagName ("RoomDescription");
		XmlNodeList roomItemList = roomData.GetElementsByTagName("RoomItem");
		XmlNodeList roomItemNeededList = roomData.GetElementsByTagName("GetInRoom");
		XmlNodeList roomItemItemList = roomData.GetElementsByTagName("GetRoomItem");
        XmlNodeList Direction = roomData.GetElementsByTagName("Direction");

        // Takes data from XMLItem to individual lists.
        XmlNodeList itemIDList = itemData.GetElementsByTagName("ItemID");
		XmlNodeList itemNameList = itemData.GetElementsByTagName("ItemName");
        XmlNodeList itemMergeWith = itemData.GetElementsByTagName("ItemMergeWith");
        XmlNodeList itemMergeTo = itemData.GetElementsByTagName("ItemMergeTo");


        // Add all item information to the specific items.
		for (int i = 0; i < itemIDList.Count; i++) 
		{
            item = new Item();

            item._itemID = Convert.ToInt32(itemIDList[i].InnerText);
            item._itemName = itemNameList[i].InnerText;
            _items.Add(item);            
		}

        //Adds information to Items which item is needed to merge.
        for (int i = 0; i < itemIDList.Count; i++)
        {
            _items[i]._mergeWithItem = SetItem(Convert.ToInt32(itemMergeWith[i].InnerText));
            _items[i]._mergeToItem = SetItem(Convert.ToInt32(itemMergeTo[i].InnerText));
        }

        // Adds all roominformation to the specific rooms.
        for (int i = 0; i < roomNameList.Count; i++) 
		{
            List<Direction> dirs = new List<Direction>();
            room = new Room();

            //Takes integer or string from XML and sets its to room variable.
            room._roomID = Convert.ToInt32(roomIDList[i].InnerText);
			room._roomName = roomNameList[i].InnerText;
			room._roomText = roomTextList[i].InnerText;
			room._roomDescription = roomDescriptionList[i].InnerText;

            //Takes integer from XML and changes integer in function to item from _items List
			room._item = SetItem(Convert.ToInt32(roomItemList[i].InnerText));
			room._requiredItemToEnter = SetItem(Convert.ToInt32(roomItemNeededList[i].InnerText));
			room._requiredItemForItem = SetItem(Convert.ToInt32(roomItemItemList[i].InnerText));

            //Takes string from data and converts it to enum list of Direction enum.
            foreach (string s in Direction[i].InnerText.Split(','))
            {
                Direction x = (Direction)(Enum.Parse(typeof(Direction), s.Trim()));
                dirs.Add(x);
            }
            room._directions = dirs;

            //Add Rooms to _room List;
            _rooms.Add(room);
		}
	}

    //returns item from depending on integer input, 0 = null.
    private Item SetItem(int i)
    {
        if( i != 0)
        {
            Item it = _items[i-1];
            return it;
        }
        else
        {
            return null;
        }
    }

    //returns room from depending on integer input, 0 = null.
    private Room SetRoom(int i)
    {
        if (i != 0)
        {
            Room currentRoom = _rooms[i];
            return currentRoom;
        }
        else
        {
            return null;
        }
    }

    #endregion PRIVATE_FUNCTIONS

    #region PUBLIC_CHANGETEXT_METHODS

    //Changes text according to currentroom.
    public void SetRoomText(Room room){
		roomName.text = room._roomName;
		roomText.text = room._roomText;
	}

    //Changes description text
	public void DescriptionText (string text)
	{
		itemDescription.text = text;
		Invoke("ResetDescriptionText", resetDescriptionTimer); // Change this later, depending of test outcome or make it a static IEnumerator
	}

	//Resets description text
	private void ResetDescriptionText()
	{
		itemDescription.text = string.Empty;
	}

    public void AddActionToQueue(string action)
    {
        if (count < maxCount)
        {
            count++;
            performedAction.Enqueue(action);
        }
        else
        {
            performedAction.Dequeue();
            performedAction.Enqueue(action);
        }

        SetDescriptionText();
        Invoke("RemovePerformedAction", 10);
    }

    private void RemovePerformedAction()
    {
        if(count > 0)
        {
            count--;
            performedAction.Dequeue();
            SetDescriptionText();
            Invoke("RemovePerformedAction", 10);
        }
        else
        {
            CancelInvoke("RemovePerformedAction");
        }
    }

    private void SetDescriptionText()
    {
        itemDescription.text = string.Empty;

        foreach (string s in performedAction)
        {
            itemDescription.text += s + "\n";
        }
    }


    #endregion PUBLIC_CHANGETEXT_METHODS
}

