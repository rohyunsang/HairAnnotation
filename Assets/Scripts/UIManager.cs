using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// ��Ȳ : ��ư�� ������ �� InitScreen�� ��Ȱ��ȭ 


public class UIManager : MonoBehaviour
{
    public Button _saveButton;
    public RawImage _rawImage;

    void Awake()
    {
        _saveButton.onClick.AddListener(TextureWrite);
    }

    private void TextureWrite()
    {
        Texture2D texture = ConvertToTexture2D(_rawImage.texture);
        
        PaintEdgesBlack(texture, 10); // �����ڸ� 10 �ȼ��� ���������� ĥ�ϱ�
        
        _rawImage.texture = texture; // ��ȯ�� �ؽ�ó�� �ٽ� RawImage�� ����

    }

    private Texture2D ConvertToTexture2D(Texture source)
    {
        Texture2D texture2D = source as Texture2D;
        if (texture2D == null)
        {
            Debug.LogError("Provided Texture is not a Texture2D or cannot be cast.");
            return null;
        }
        Texture2D newTexture = new Texture2D(texture2D.width, texture2D.height, texture2D.format, false);
        newTexture.SetPixels(texture2D.GetPixels());
        newTexture.Apply();
        return newTexture;
    }

    private void PaintEdgesBlack(Texture2D texture, int borderWidth)
    {
        Color black = Color.black;
        int width = texture.width;
        int height = texture.height;

        // Top & Bottom borders
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < borderWidth; y++)
            {
                texture.SetPixel(x, y, black); // Bottom
                texture.SetPixel(x, height - y - 1, black); // Top
            }
        }

        // Left & Right borders
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < borderWidth; x++)
            {
                texture.SetPixel(x, y, black); // Left
                texture.SetPixel(width - x - 1, y, black); // Right
            }
        }

        texture.Apply(); // ��������� ����
    }

    private void ImageSave()
    {
        Debug.Log("Save Button Callback");

        Texture2D textureToSave = ConvertToTexture2DImageSave(_rawImage.texture);  // RawImage�� Texture
        
        SaveTextureToDesktop(textureToSave);
    }

    private Texture2D ConvertToTexture2DImageSave(Texture source) // Texture -> Texture2D �� �ٲ��ִ� �ڵ� .
    {
        Texture2D texture2D = source as Texture2D;
        if (!texture2D)
        {
            Debug.LogError("Provided Texture is not a Texture2D.");
            return null;
        }

        return texture2D;
    }

    private void SaveTextureToDesktop(Texture2D texture)
    {
        byte[] imageBytes = texture.EncodeToPNG(); // texture to PNG -> byte[] 

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = "savedImage.png";
        string fullPath = Path.Combine(desktopPath, fileName);

        File.WriteAllBytes(fullPath, imageBytes);

        Debug.Log("Image saved to Desktop at: " + fullPath);
    }


    


    

    private void StartButtonCallback()
    {
        // 1
        //Debug.Log("Start Button Pressed");
        //_initScreen.SetActive(false);

        // 2 
        // ������Ʈ �̸����� ã��
        // GameObject.Find("InitScreen").SetActive(false);

        // 3
        // ��ũ��Ʈ ������ �ִ� ��� ã��.
        FindObjectOfType<MainScreenConroller>().TestFunc();

        // 4 
        // ��ũ��Ʈ�� ������ �ִ� ��ü ã��. 
        GameObject mainScreenObject =
            FindObjectOfType<MainScreenConroller>().gameObject;
        Debug.Log(mainScreenObject.name);
    }
}
