using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AdjustImage : MonoBehaviour
{
    public GameObject FileBrowserObj;

    public byte[] adjustmentImageByte;
    public Texture2D adjustImageTexture;
    public RawImage adjustRawImage;

    public const int rawImageHeight = 800;

    public int pixelWidth = 0;
    public int pixelHeight = 0;

    public float[] circleLengths = new float[4];
    public float circleLength = 0f;
    public float adjustImagePixelLength = 0f;

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

    public void CalculateAdjustImagePixelLength() // using CheckBtn in DotListImage in AdjustingPanel
    {

        adjustImagePixelLength = circleLength / (float)rawImageHeight * (float)pixelHeight;
    }

    public void MakePixelLengthFile()  // using CheckBtn in DotListImage in AdjustingPanel
    {
        string path = FileBrowserObj.GetComponent<FileBrowserTest>().directoryPath + "/조정용.txt";

        // 파일이 이미 존재하는 경우 삭제
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        File.AppendAllText(path,adjustImagePixelLength.ToString());
    }
}


