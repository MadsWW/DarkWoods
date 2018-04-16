using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public string _buttonInfo;
    public GameObject _infoButton;

    private GameProgress gameProg;

    public int _healthCost;

    private void Start()
    {
        gameProg = FindObjectOfType<GameProgress>();
    }

    public void OnPointerEnter()
    {
        Text text = _infoButton.GetComponent<Text>();
        text.text = _buttonInfo;
    }


    public void OnPointerExit()
    {
        Text text = _infoButton.GetComponent<Text>();
        text.text = "Tip: Hover over Buttons.";
    }

    public void PassDamage()
    {
        gameProg.ChangeHealth(_healthCost);
    }
}
