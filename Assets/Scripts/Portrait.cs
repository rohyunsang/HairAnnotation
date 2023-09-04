using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public GameObject JsonParsingManagerObj;
    public GameObject checkingImage;

    public void PortraitClick()
    {
        JsonParsingManagerObj = GameObject.Find("JsonParsingManager");
        string currentId = transform.name; // 여기서 transform.name이 해당 포트레이트의 id 값과 일치한다고 가정합니다.
        JsonParsingManagerObj.GetComponent<JsonParsing>().QueueManager(currentId);
    }
}