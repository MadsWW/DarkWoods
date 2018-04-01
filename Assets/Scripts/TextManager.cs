using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;


public enum Direction { North, East, South, West };

public class TextManager : MonoBehaviour {

    public GameProgress gameProg;

    public Text roomName;
	public Text roomText;
	public Text itemDescription;

	public List<Room> _rooms = new List<Room>();
	public List<Item> _items = new List<Item>();

	public TextAsset xmlRoom;
	public TextAsset xmlItem;

	public string _noItemText;
    public string _cantEnterRoom;
    public string _cantPickupItem;
    public string _pickUpText;

    public int resetDescriptionTimer;


	private void Start ()
	{
		FillRoomItemList();
		SetRoomText (_rooms[0]);
		ResetDescriptionText ();

        SetupGame();

    }

    private void SetupGame()
    {
        Room room = _rooms[0];
        gameProg.SetDirectionButtons(room);
    }


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
		XmlNodeList roomItemNeededList = roomData.GetElementsByTagName("NeedItem");
		XmlNodeList roomItemItemList = roomData.GetElementsByTagName("NeedItemItem");
        XmlNodeList Direction = roomData.GetElementsByTagName("Direction");

        // Takes data from XMLItem to individual lists.
        XmlNodeList itemIDList = itemData.GetElementsByTagName("ItemID");
		XmlNodeList itemNameList = itemData.GetElementsByTagName("ItemName");


        // Add all item information to the specific items.
		for (int i = 0; i < itemIDList.Count; i++) 
		{
			item = new Item(Convert.ToInt32(itemIDList[i].InnerText), itemNameList[i].InnerText);
			_items.Add(item);
            
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
    public Room SetRoom(int i)
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


	//Changes text according to currentroom.
	public void SetRoomText(Room room){
		roomName.text = room._roomName;
		roomText.text = room._roomText;
	}

	// Set Description of room into text area
	public void SetDescriptionText(int roomNumber)
	{
		itemDescription.text = _rooms[roomNumber]._roomDescription;
		Invoke("ResetDescriptionText", resetDescriptionTimer);
	}

	public void DescriptionText (string text)
	{
		itemDescription.text = text;
		Invoke("ResetDescriptionText", resetDescriptionTimer);
	}

	//Resets description text
	private void ResetDescriptionText()
	{
		itemDescription.text = string.Empty;
	}
}

