using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    Item _buttonItem;
    Text _buttonText;
    Button _button;
    public bool selected = false;

    public static Item selectedItem;
    public static Item mergeItem;
    public static Button selectedButton;
    public static Button mergeButton;


    private void Start()
    {
        _buttonText = GetComponent<Text>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetItem);
    }

    //Gets called when button is instantiated.
    public void SetButtonText(Item it)
    {
        _buttonText = GetComponentInChildren<Text>();
        _buttonItem = it;
        _buttonText.text = it._itemName;
    }

    //Selects button depending on it state.
    private void SetItem()
    {
        if (selected)
        {
            selected = false;
            ResetButton();
        }
        else if (!selected && selectedItem == null)
        {
            selected = true;
            SetSelectedItem();
        }
        else if (!selected && selectedItem != null)
        {
            selected = true;
            SetMergeItem();
        }
    }

    //Reset both static item/buttons.
    public static void ResetButton()
    {
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().color = Color.white;
            selectedButton.GetComponent<ItemButton>().selected = false;
            selectedItem = null;
        }

        if(mergeItem != null)
        {
            mergeButton.GetComponent<Image>().color = Color.white;
            mergeButton.GetComponent<ItemButton>().selected = false;
            mergeItem = null;
        }

    }

    //Sets Selected Item/Button
    private void SetSelectedItem()
    {
        if (selectedItem != null)
        {
            selectedButton.GetComponent<Image>().color = Color.white;
        }
            selectedItem = _buttonItem;
            selectedButton = _button;
            GetComponent<Image>().color = Color.green;
    }

    //Sets Merge Item/Button
    private void SetMergeItem()
    {
        if(mergeItem != null)
        {
            mergeButton.GetComponent<Image>().color = Color.white;
        }
        mergeItem = _buttonItem;
        mergeButton = _button;
        GetComponent<Image>().color = Color.yellow;
    }
}
