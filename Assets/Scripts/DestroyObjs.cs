using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DestroyObjs : MonoBehaviour
{
    public GameObject faceImage;
    public GameObject jsonManager; //square,image.
    public GameObject mainScrollViewContent;
    public GameObject initScrollViewContent;

    public void DestroyBtn()
    {
        // faceImage�� �ڽ� ������Ʈ ��� ����

        jsonManager.GetComponent<JsonParsing>().ClearObjs();

        // mainScrollViewContent�� �ڽ� ������Ʈ ��� ����
        foreach (Transform child in mainScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // initScrollViewContent�� �ڽ� ������Ʈ ��� ����
        foreach (Transform child in initScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
