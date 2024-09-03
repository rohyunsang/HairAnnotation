using System.IO;
using UnityEngine;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

public class ManualJson : MonoBehaviour
{
    public void SaveData(string json)
    {
        string filePath = FindObjectOfType<App>().directoryPath;
        string fileFolder = FindObjectOfType<App>().currentFolderName;
        string fileName = FindObjectOfType<App>().currentImageName;
        string directory = Path.Combine(filePath, "Output", fileFolder);
        string path = Path.Combine(filePath, "Output", fileFolder, fileName + ".json");

        // ��ο� �ִ� ���丮�� �������� ������ ����
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        System.IO.File.WriteAllText(path, json);
        Debug.Log("Data saved to " + path);
    }
}