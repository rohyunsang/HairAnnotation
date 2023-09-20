using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject InitPanel;
    public GameObject OptionPanel;
    public GameObject MainPanel;

    public void OnInitPanel(){
        InitPanel.SetActive(true);
    }
    public void OffInitPanel(){
        InitPanel.SetActive(false);
    }
    public void OnOptionPanel(){
        OptionPanel.SetActive(true);
    }
    public void OffOptionPanel(){
        OptionPanel.SetActive(false);
    }

    public void OnMainPanel(){
        MainPanel.SetActive(true);
    }
    public void OffMainPanel(){
        MainPanel.SetActive(false);
    }
}
