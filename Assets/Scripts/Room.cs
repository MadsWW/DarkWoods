﻿using System.Collections.Generic;

//TODO Make properties to all public variables.

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
    public bool _itemgrabbed = false;

    public List<Direction> _directions;
}
