using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchStatusObj : MonoBehaviour
{
    Status status;
    PanelManager panelManager;

    private void Start()
    {
        status = GameObject.Find("StatusManager").GetComponent<Status>();
        panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
    }

    public void HandleButtonClick(Button clickedButton)
    {
        Debug.Log("buttonClick");
        panelManager.OnStatusPanel();
        switch (clickedButton.name)
        {
            case "forehead":
                status.OnForehead();
                break;
            case "glabellus":
                status.OnGlabellus();
                break;
            case "l_peroucular":
                status.OnLPeriocular();
                break;
            case "r_peroucular":
                status.OnRPeriocular();
                break;
            case "l_cheek":
                status.OnLCheek();
                break;
            case "r_cheek":
                status.OnRCheek();
                break;
            case "lip":
                status.OnLip();
                break;
            case "chin":
                status.OnChin();
                break;
            default:
                Debug.LogError("Unknown button name: " + clickedButton.name);
                break;
        }
    }
}