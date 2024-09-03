using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Attempt to find an existing instance in the scene
                instance = FindObjectOfType<UIManager>();

                // Create a new instance if one doesn't already exist
                if (instance == null)
                {
                    GameObject newObj = new GameObject("UIManager");
                    instance = newObj.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    public Button _manualButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy any duplicate objects that might be created
        }

        _manualButton.onClick.AddListener(OnClickManualButton);
    }

    private void OnClickManualButton()
    {
        FindObjectOfType<App>().manualMode = true;
    }
}