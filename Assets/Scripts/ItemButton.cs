using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{

    Item _buttonItem;
    Text _buttonText;
    Button _button;
    public bool selected = false;


    private void Start()
    {
        _buttonText = GetComponent<Text>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetItem);
    }

    public void SetButtonText(Item it)
    {
        _buttonText = GetComponentInChildren<Text>();
        _buttonItem = it;
        _buttonText.text = it._itemName;
    }

    private void SetItem()
    {
        if (selected)
        {
            print("deselecting");
            selected = !selected;
            ResetButton();

        }
        else if (!selected && GameProgress.selectedItem == null)
        {
            selected = !selected;
            SetSelectedItem();
        }
        else if (!selected && GameProgress.selectedItem != null)
        {
            selected = !selected;
            SetMergeItem();
        }

    }


    private void ResetButton()
    {
        GameProgress.selectedButton.GetComponent<Image>().color = Color.white;
        GameProgress.selectedButton.GetComponent<ItemButton>().selected = false;
        GameProgress.selectedItem = null;

        if(GameProgress.mergeItem != null)
        {
            GameProgress.mergeButton.GetComponent<Image>().color = Color.white;
            GameProgress.mergeButton.GetComponent<ItemButton>().selected = false;
            GameProgress.mergeItem = null;
        }

    }
    private void SetSelectedItem()
    {
        if (GameProgress.selectedItem != null)
        {
            GameProgress.selectedButton.GetComponent<Image>().color = Color.white;
        }
            GameProgress.selectedItem = _buttonItem;
            GameProgress.selectedButton = _button;
            GetComponent<Image>().color = Color.green;
    }

    private void SetMergeItem()
    {
        if(GameProgress.mergeItem != null)
        {
            GameProgress.mergeButton.GetComponent<Image>().color = Color.white;
        }
        GameProgress.mergeItem = _buttonItem;
        GameProgress.mergeButton = _button;
        GetComponent<Image>().color = Color.yellow;
    }





}
