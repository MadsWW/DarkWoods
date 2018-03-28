using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgress : MonoBehaviour {

	public TextManager _textManager;

	private List<Item> _inventory = new List<Item>();

	public string _cantEnterRoom;
	public string _cantPickupItem;
	public string _pickUpText;
	public int _itemIDForWin;


	private void Start ()
	{
		_inventory.Add(new Item(0, "Nothing"));
	}
	// Checks if there is item in room and runs functions to add item to inventory.
	public void TakeItemToInventory (int roomNumber)
	{
		if (_textManager.itemTaken [roomNumber] == false) 
		{
			_textManager.itemTaken [roomNumber] = true;
			_inventory.Add(AddItemToInventory(roomNumber));
		} 
		else 
		{
			_textManager.DescriptionText(_textManager.noItemText);
		}
	}

	// Adds item to the players inventory.
	private Item AddItemToInventory (int roomNumber)
	{
		foreach (Item it in _textManager._items) 
		{
			if (_textManager._rooms [roomNumber]._item == it._itemID) 
			{
				_textManager.DescriptionText(_pickUpText + it._itemName);
				_textManager.SetInventoryText(it);
				CheckForWin(it);
				return it;
			} 
			
		}
		return null;
	}

	// Checks if the player has got the required item to enter the next room.
	public bool CanEnterNextRoom (int roomNumber)
	{
		foreach (Item it in _inventory) {

			bool needsItem2Enter = _textManager._rooms [roomNumber -1]._requiredItemToEnter == it._itemID;
			bool noItemRequired = _textManager._rooms [roomNumber-1 ]._requiredItemToEnter == 0;
			
			if (needsItem2Enter || noItemRequired) 
			{	
				if (!_textManager.roomEntered [roomNumber -1] && _textManager._rooms [roomNumber - 1]._requiredItemToEnter != 0) 
				{
					_textManager.DescriptionText ("Used: " + it._itemName);
					_textManager.roomEntered [roomNumber -1] = true;
				}
				return true;
			} 
		}
		_textManager.DescriptionText(_cantEnterRoom);
		Invoke("Rip", 4f);
		return false;
	}


	//Checks if player has got required item in inventory to grab the item in the room.
	public bool CanGrabItemInRoom (int roomNumber)
	{
		foreach (Item it in _inventory) 
		{
			if (_textManager._rooms [roomNumber]._requiredItemForItem == it._itemID || _textManager._rooms[roomNumber]._requiredItemForItem == 0) 
			{
				return true;
			} 
		}
		_textManager.DescriptionText(_cantPickupItem);
		Invoke("Rip", 4f);
		return false;
	}

	private void CheckForWin (Item it)
	{
		if (it._itemID == _itemIDForWin) 
		{
			SceneManager.LoadScene("WinScene"); 
		}
	}

	private void Rip ()
	{
		SceneManager.LoadScene("LoseScene"); 
	}
}
