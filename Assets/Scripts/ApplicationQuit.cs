using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationQuit : MonoBehaviour
{
    
    public GameObject checkQuitImage;

    public void IsQuitApplication()
    {
        checkQuitImage.SetActive(true);
    }
    
    public void OffCheckQuitImage()
    {
        checkQuitImage.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
