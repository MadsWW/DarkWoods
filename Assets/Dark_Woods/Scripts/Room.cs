using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room {

	public int _roomID;
	public string _roomName;
	public string _roomText;
	public string _roomDescription;
	public string _roomDirections;
	public int _item; 					//Needs to be class Item _item
	public int _requiredItemToEnter; 	//Needs to be class Item _requiredItemToEnter
	public int _requiredItemForItem; 	//Needs to be class Item _requiredItemForItem

	//private enum Directions{North, South, East, West};
}
