using UnityEngine;
using System.Collections;
using System.IO;
using TMPro;
using SimpleFileBrowser;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class FileBrowserTest : MonoBehaviour
{
    public Dictionary<string, string> jsonStrings = new Dictionary<string, string>(); // fileName, fileBytes

    public GameObject jsonManager;

    public string filePath = "";

    public void ShowFileBrowser()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Files", ".jpg", ".png", ".json"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".json");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                string extension = Path.GetExtension(FileBrowser.Result[i]);

                if (extension == "")
                {
                    // ��� .json ���� ó��
                    List<string> jsonFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.json");
                    foreach (string jsonFile in jsonFiles)
                    {
                        Debug.Log("Processing JSON file: " + Path.GetFileName(jsonFile));  // JSON ���� �̸� �����
                        if (Path.GetFileName(jsonFile).Contains("pimple"))
                            continue;
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jsonFile);
                        jsonStrings[Path.GetFileName(jsonFile)] = System.Text.Encoding.UTF8.GetString(bytes);

                    }

                    // ��� .jpg ���� ó��
                    List<string> jpgFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.jpg");
                    foreach (string jpgFile in jpgFiles)
                    {
                        string fileName = Path.GetFileName(jpgFile);

                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jpgFile);
                        jsonManager.GetComponent<JsonParsing>().MakeImageStringArray(bytes);

                        // .jpg ������ ���� ���丮�� �����ɴϴ�.
                        string currentDirectory = Path.GetDirectoryName(jpgFile);
                        filePath = Path.GetDirectoryName(jpgFile);
                        Debug.Log("Current Directory of " + Path.GetFileName(jpgFile) + ": " + currentDirectory);
                    }
                }
                var ordered = jsonStrings.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                jsonStrings = ordered;
                foreach (KeyValuePair<string, string> entry in jsonStrings)
                {
                    Debug.Log("Key: " + entry.Key);
                }
                // ���ĵ� ��ųʸ����� ������ ������ ���� �����ɴϴ�.
                string lastJsonValue = jsonStrings.LastOrDefault().Value;

                // ������ ������ ���� MakeJsonArray �޼��忡 �����մϴ�.
                jsonManager.GetComponent<JsonParsing>().MakeJsonArray(lastJsonValue);
                jsonManager.GetComponent<JsonParsing>().CheckingFileCount();
                string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[i]));
                FileBrowserHelpers.CopyFile(FileBrowser.Result[i], destinationPath);
            }
        }
    }

    public List<string> GetAllFilesInDirectory(string directoryPath, string searchPattern)
    {
        List<string> files = new List<string>();

        // ���� ���丮���� ���ϵ��� �����ɴϴ�.
        files.AddRange(Directory.GetFiles(directoryPath, searchPattern));

        // ��� ���� ���丮�� ������ �� ���� ���丮�� ���� ��������� �� �Լ��� ȣ���մϴ�.
        foreach (string subDirectory in Directory.GetDirectories(directoryPath))
        {
            files.AddRange(GetAllFilesInDirectory(subDirectory, searchPattern));
        }

        return files;
    }
}