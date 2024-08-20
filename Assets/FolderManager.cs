using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Folder, File 가져올때 사용 
using UnityEngine.UI;

public class FolderManager : MonoBehaviour
{
    public GameObject _layout;
    public GameObject _imagePrefab;

    private readonly List<string> imageExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff" };

    void Start()
    {
        InvokeRepeating("RepeatedGetImages", 0f, 2f); 
    }

    void RepeatedGetImages()
    {
        List<string> imageFiles = GetImagesFromFolder("Test");
        foreach (var imageFile in imageFiles)
        {
            Debug.Log("Found image file: " + imageFile);
        }
    }

    List<string> GetImagesFromFolder(string folderName)
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string folderPath = Path.Combine(desktopPath, folderName);

        List<string> imageFiles = new List<string>();

        if (Directory.Exists(folderPath))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            FileInfo[] files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            foreach (FileInfo file in files)
            {
                if (imageExtensions.Contains(file.Extension.ToLower()))
                {
                    imageFiles.Add(file.FullName);
                    StartCoroutine(LoadImage(file.FullName)); // Coroutine -> 병렬처리 
                    // 유니티 엔진이 이게 싱글 스레드 기반이라 -> 멀티 스레딩이 안돼서
                }
            }
        }
        else
        {
            Debug.LogError("Folder not found: " + folderPath);
        }

        return imageFiles;
    }

    IEnumerator LoadImage(string imagePath)
    {
        Texture2D texture = new Texture2D(2, 2);
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        if (imageBytes.Length > 0)
        {
            texture.LoadImage(imageBytes); // 이미지 데이터로 텍스처 생성
            texture.Apply();

            // texture2d를 이용해서 sprite를 만든다. 
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); 
            GameObject imagePrefab = Instantiate(_imagePrefab, _layout.transform);
            imagePrefab.name = Path.GetFileName(imagePath);
            imagePrefab.GetComponent<Image>().sprite = newSprite;
        }
        yield return null;
    }
}
