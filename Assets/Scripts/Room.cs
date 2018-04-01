using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room
{  
    public int _roomID;
	public string _roomName;
	public string _roomText;
	public string _roomDescription;
	public string _roomDirections;
	public Item _item;
	public Item _requiredItemToEnter;
	public Item _requiredItemForItem;

    public List<Direction> _directions;
}
