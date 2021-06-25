using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPopup : Popup
{
    public void OnButtonPressed()
    {
        Debug.Log("BUY");
        Close();
    }

    public void OnCloseButtonPressed()
    {
        Close();
    }
}