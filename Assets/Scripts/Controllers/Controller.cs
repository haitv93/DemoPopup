using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public void ShowAlertPopup()
    {
        PopupController.OpenPopup<AlertPopup>("Popups/AlertPopup", popup =>
        {
            popup.OnClose.AddListener(() =>
            {
                ShowBuyPopup();
            });
        });
    }

    public void ShowBuyPopup()
    {
        Debug.Log("Show Buy Popup");
        PopupController.OpenPopup<ShopPopup>("Popups/ShopPopup");
    }
}