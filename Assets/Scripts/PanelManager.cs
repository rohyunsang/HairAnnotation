using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject InitPanel;
    public GameObject OptionPanel;
    public GameObject SavePanel;
    public GameObject LoginPanel;
    public GameObject StatusPanel;


    public GameObject faceField;

    public GameObject ImageClickInfo;
    public GameObject ImageEditInfo;

    public GameObject SaveInfo;
    public GameObject SaveCompleteInfo;
    public GameObject pimpleSaveCompleteInfo;

    public Text uncheckingStatusText;

    public void OnPimpleSaveCompleteInfo()
    {
        pimpleSaveCompleteInfo.SetActive(true);
    }
    public void OffPimpleSaveCompleteInfo()
    {
        pimpleSaveCompleteInfo.SetActive(false);
    }

    public void OffSaveInfo()
    {
        SaveInfo.SetActive(false);
        uncheckingStatusText.text = "\n 피부 상태를 체크해주세요. \n";
    }

    public void OnSaveInfo()
    {
        SaveInfo.SetActive(true);
    }

    public void OnSaveCompleteInfo()
    {
        SaveCompleteInfo.SetActive(true);
    }
    
    public void OffSaveCompleteInfo()
    {
        SaveCompleteInfo.SetActive(false);
    }

    public void OnImageEditInfo()
    {
        ImageEditInfo.SetActive(true);
    }

    public void OffImageEditInfo()
    {
        ImageEditInfo.SetActive(false);
    }

    public void OnStatusPanel()
    {
        StatusPanel.SetActive(true);
    }
    public void OffStatusPanel()
    {
        StatusPanel.SetActive(false);
    }

    public void OffImageClickInfo()
    {
        Invoke("InvokeOffImageClickInfo", 3f);
    }

    public void InvokeOffImageClickInfo()
    {
        ImageClickInfo.SetActive(false);
    }
    public void OnInitPanel()
    {
        InitPanel.SetActive(true);
    }

    public void OnOptionPanel()
    {
        OptionPanel.SetActive(true);
    }
    public void OnSavePanel()
    {
        SavePanel.SetActive(true);
    }

    public void OnLoginPanel()
    {
        LoginPanel.SetActive(true);
    }
    public void OffLoginPanel()
    {
        LoginPanel.SetActive(false);
    }
    public void OffSavePanel()
    {
        SavePanel.SetActive(false);
    }

    public void OffInitPanel()
    {
        InitPanel.SetActive(false);
    }

    public void OffOptionPanel()
    {
        OptionPanel.SetActive(false);
    }
    
    public void DeleteObjects()
    {
        foreach (Transform child in faceField.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
