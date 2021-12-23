using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPopup : Popup
{
    public void OnButtonPressed()
    {
        Debug.Log("PLAY");
    }

    public void OnCloseButtonPressed()
    {
        Close();
    }
}