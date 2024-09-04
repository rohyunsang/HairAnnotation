using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using System.Collections.Generic;
using System.Linq;

public class FileBrowserTest : MonoBehaviour
{
    public Dictionary<string, Dictionary<string, byte[]>> jpgDict = new Dictionary<string, Dictionary<string, byte[]>>();

    public string directoryPath = "";
    public GameObject AdjsustImageObj;
    public GameObject CustomFileUIObj;

    public List<string> allJpgFileNames = new List<string>(); // All .jpg files including those in subdirectories
    public List<byte[]> allJpgFiles = new List<byte[]>();

    public List<string> allJsonFileNames = new List<string>();
    public List<byte[]> allJsonFiles = new List<byte[]>();
    public Dictionary<string, byte[]> jsonDict = new Dictionary<string, byte[]>();

    private List<string> allDirectoryNames = new List<string>(); // Names of all directories
    

    public void ShowFileBrowserOnlyJPG()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Files", ".jpg", ".png", ".json"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".json");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutineJPG());
    }
    IEnumerator ShowLoadDialogCoroutineJPG()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Load Files", "Load");

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                string extension = Path.GetExtension(FileBrowser.Result[i]);

                if (extension == ".jpg")
                {
                    string jpgFile = FileBrowser.Result[i];
                    string fileName = Path.GetFileName(jpgFile);

                    AdjsustImageObj.GetComponent<AdjustImage>().adjustmentImageByte = FileBrowserHelpers.ReadBytesFromFile(jpgFile);

                    string currentDirectory = Path.GetDirectoryName(jpgFile);
                    directoryPath = Path.GetDirectoryName(jpgFile);
                    Debug.Log("Current Directory of " + Path.GetFileName(jpgFile) + ": " + currentDirectory);
                }
            }

            AdjsustImageObj.GetComponent<AdjustImage>().InitRawImage();
        }
    }

    public void ShowFileBrowser()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Files", ".jpg", ".png", ".json", ".jpeg"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
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
                    // ������ ��ΰ� ���丮�� ���
                    Debug.Log("Selected a directory: " + FileBrowser.Result[i]);
                    FindObjectOfType<App>().directoryPath = FileBrowser.Result[i];

                    // .jpg ���ϰ� .json ������ ��� ó���ϱ� ���� �Լ� ȣ��
                    ProcessDirectory(FileBrowser.Result[i]);

                    // UI ������Ʈ
                    CustomFileUIObj.GetComponent<CustomFileUI>().ScrollViewInstantiate(jpgDict);

                    string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[i]));
                    FileBrowserHelpers.CopyFile(FileBrowser.Result[i], destinationPath);
                }
            }
        }
    }
    public void ProcessDirectory(string directoryPath)
    {
        // .jpg ���� ó��
        List<string> jpgFiles = GetAllFilesInDirectory(directoryPath, "*.jpg");
        Debug.Log(jpgFiles.Count + " .jpg files found in the directory.");
        List<string> sortedJpgFiles = jpgFiles.OrderBy(Path.GetFileName).ToList();  // sorting

        foreach (string jpgFile in sortedJpgFiles)
        {
            byte[] fileBytes = FileBrowserHelpers.ReadBytesFromFile(jpgFile);
            string fileName = Path.GetFileName(jpgFile);
            string currentDirectory = Path.GetDirectoryName(jpgFile);
            string currentDirectoryName = Path.GetFileName(currentDirectory);

            if (!jpgDict.ContainsKey(currentDirectoryName))
            {
                jpgDict[currentDirectoryName] = new Dictionary<string, byte[]>();
            }
            jpgDict[currentDirectoryName].Add(fileName, fileBytes);

            allJpgFileNames.Add(currentDirectoryName + "_" + fileName);
            allJpgFiles.Add(fileBytes);
        }

        // .json ���� ó��
        List<string> jsonFiles = GetAllFilesInDirectory(directoryPath, "*.json");
        Debug.Log(jsonFiles.Count + " .json files found in the directory.");

        foreach (string jsonFile in jsonFiles)
        {
            byte[] jsonFileBytes = FileBrowserHelpers.ReadBytesFromFile(jsonFile);
            allJsonFiles.Add(jsonFileBytes);

            string jsonFileName = Path.GetFileName(jsonFile);
            string currentDirectory = Path.GetDirectoryName(jsonFile);
            string currentDirectoryName = Path.GetFileName(currentDirectory);

            // ���� �̸��� allJsonFileNames ����Ʈ�� �߰�
            allJsonFileNames.Add(jsonFileName);
            Debug.Log(jsonFileName + "Debug");

            // jsonDict�� ���� �̸��� byte[] ������ �߰�
            if (!jsonDict.ContainsKey(jsonFileName))
            {
                jsonDict.Add(jsonFileName, jsonFileBytes);
            }
        }
    }

    public List<string> GetAllFilesInDirectory(string directoryPath, string searchPattern)
    {
        List<string> files = new List<string>();

        files.AddRange(Directory.GetFiles(directoryPath, searchPattern));

        foreach (string subDirectory in Directory.GetDirectories(directoryPath))
        {
            allDirectoryNames.Add(Path.GetFileName(subDirectory));
            files.AddRange(GetAllFilesInDirectory(subDirectory, searchPattern));
        }

        return files;
    }
}