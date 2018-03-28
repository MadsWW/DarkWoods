using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Xml.Linq;



public class TextManager : MonoBehaviour {

	public Text roomName;
	public Text roomText;
	public Text itemDescription;
	public Text inventory;

	public List<Room> _rooms = new List<Room>();
	public List<Item> _items = new List<Item>();

	public TextAsset xmlRoom;
	public TextAsset xmlItem;

	public string noItemText;
	public int resetDescriptionTimer;

	public bool[] itemTaken;
	public bool[] roomEntered;



	private void Start ()
	{
		FillRoomItemList();
		SetItemTakenArray ();
		SetRoomText (0);
		ResetDescriptionText ();

		inventory.text = "";

	}



	private void FillRoomItemList ()
	{
		Item item;
		Room room;

		XmlDocument roomData = new XmlDocument();
		XmlDocument itemData = new XmlDocument();

		roomData.LoadXml (xmlRoom.text);
		itemData.LoadXml(xmlItem.text);

		XmlNodeList roomNameList = roomData.GetElementsByTagName ("RoomName");
		XmlNodeList roomTextList = roomData.GetElementsByTagName ("RoomText");
		XmlNodeList roomDescriptionList = roomData.GetElementsByTagName ("RoomDescription");
		XmlNodeList roomDirectionsList = roomData.GetElementsByTagName ("Direction");
		XmlNodeList roomItemList = roomData.GetElementsByTagName("RoomItem");
		XmlNodeList roomItemNeededList = roomData.GetElementsByTagName("NeedItem");
		XmlNodeList roomItemItemList = roomData.GetElementsByTagName("NeedItemItem");

		XmlNodeList itemIDList = itemData.GetElementsByTagName("ItemID");
		XmlNodeList itemNameList = itemData.GetElementsByTagName("ItemName");

		for (int i = 0; i < itemIDList.Count; i++) 
		{
			item = new Item(Convert.ToInt32(itemIDList[i].InnerText), itemNameList[i].InnerText);
			_items.Add(item);
		}

		for (int i = 0; i < roomNameList.Count; i++) 
		{
			room = new Room();
			room._roomID = i+1;
			room._roomName = roomNameList[i].InnerText;
			room._roomText = roomTextList[i].InnerText;
			room._roomDescription = roomDescriptionList[i].InnerText;
			room._roomDirections = roomDirectionsList[i].InnerText;
			room._item = Convert.ToInt32(roomItemList[i].InnerText);
			room._requiredItemToEnter = Convert.ToInt32(roomItemNeededList[i].InnerText);
			room._requiredItemForItem = Convert.ToInt32(roomItemItemList[i].InnerText);

			_rooms.Add(room);
		}
	}


	//Sets bool for all items in the rooms to false !! - Make all items a class
	void SetItemTakenArray()
	{
		itemTaken = new bool[_rooms.Count];
		roomEntered = new bool[_rooms.Count];

		for (int i = 0; i < _rooms.Count; i++) 
		{
			itemTaken[i] = false;
			roomEntered[i] = false;
		}
	}

	//Changes text according to currentroom.
	public void SetRoomText(int roomNumber){
		roomName.text = _rooms[roomNumber]._roomName;
		roomText.text = _rooms[roomNumber]._roomText + "\n\n" + "Direction(s): " + _rooms[roomNumber]._roomDirections;
	}

	// Set Description of room into text area
	public void SetDescriptionText(int roomNumber)   // Could be Made into one method with method below.
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
		itemDescription.text = "";
	}

	//Puts all items into text area.
	public void SetInventoryText (Item item)
	{
		inventory.text += item._itemName + "\n\n";
	}



}

