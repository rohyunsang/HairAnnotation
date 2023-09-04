using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectAddListener : MonoBehaviour
{
    public ButtonZoom buttonZoomScript;
    public void OnClick()
    {
        buttonZoomScript = GameObject.Find("faceImage").GetComponent<ButtonZoom>();
        buttonZoomScript.OnButtonClick(GetComponent<RectTransform>());
    }
}
