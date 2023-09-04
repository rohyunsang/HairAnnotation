using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveUserData : MonoBehaviour
{
    public InputField idField;
    public Text idCheckText;
    public GameObject UserDataCheckingImage;

    public void OnLoginBtn()
    {
        idCheckText.text += idField.text;
    }
    public void OnUserDataCheckImage()
    {
        if (!idField.text.Equals(""))
            UserDataCheckingImage.SetActive(true);
    }
    public void OffUserDataCheckImage()
    {
        UserDataCheckingImage.SetActive(false);
    }
    public void DeleteUserData()
    {
        idCheckText.text = "¿Ã∏ß : ";
    }
}