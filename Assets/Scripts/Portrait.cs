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
        string currentId = transform.name; // ���⼭ transform.name�� �ش� ��Ʈ����Ʈ�� id ���� ��ġ�Ѵٰ� �����մϴ�.
        JsonParsingManagerObj.GetComponent<JsonParsing>().QueueManager(currentId);
    }
}