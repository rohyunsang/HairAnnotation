using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustImage : MonoBehaviour
{
    public byte[] adjustmentImageByte;
    public Texture2D adjustImageTexture;
    public RawImage adjustRawImage;

    public const int rawImageHeight = 800;

    public int pixelWidth = 0;
    public int pixelHeight = 0;

    public float[] circleLengths = new float[4];
    public float circleLength = 0f;

    public void InitRawImage() 
    {
        ByteToTexture();
        GetPixels();
        ScalingRawImage();
    }


    private void ByteToTexture()
    {
        if (adjustmentImageByte == null)  // checking null
            return;

        if (adjustImageTexture == null)
            adjustImageTexture = new Texture2D(2, 2);  // new

        adjustImageTexture.LoadImage(adjustmentImageByte);  //LoadImage using Byte

        if (adjustRawImage != null)  // show using RawImage
            adjustRawImage.texture = adjustImageTexture;
    }

    private void GetPixels()
    {
        pixelWidth = adjustImageTexture.width;
        pixelHeight = adjustImageTexture.height;
    }

    private void ScalingRawImage()
    {
        if (adjustImageTexture == null)
            return;

        // Calculate new width based on aspect ratio
        float newWidth = (float)pixelWidth / pixelHeight * rawImageHeight;

        // Set the size of the RawImage
        adjustRawImage.rectTransform.sizeDelta = new Vector2(newWidth, rawImageHeight);
    }

    public void CalculateAverageCircleLength(){

    }
}


