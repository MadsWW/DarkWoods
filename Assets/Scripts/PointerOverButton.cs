using UnityEngine;
using UnityEngine.UI;

public class PointerOverButton : MonoBehaviour {

    public Text text;

    public void OnPointerEnter()
    {
        text.enabled = true;
    }

    public void OnPointerExit()
    {
        text.enabled = false;
    }
}
