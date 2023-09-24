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
    public string directoryPath = "";
    public GameObject AdjsustImageObj;
    public Dictionary<string, string> jsonStrings = new Dictionary<string, string>(); // fileName, fileBytes
    public List<string> allJpgFiles = new List<string>(); // All .jpg files including those in subdirectories
    public List<string> allDirectoryNames = new List<string>(); // Names of all directories


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
                    // Process all .json files
                    List<string> jsonFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.json");
                    foreach (string jsonFile in jsonFiles)
                    {
                        if (Path.GetFileName(jsonFile).Contains("pimple"))
                            continue;
                        if (Path.GetFileName(jsonFile).Contains("face"))
                            continue;
                        Debug.Log("Processing JSON file: " + Path.GetFileName(jsonFile));
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jsonFile);
                        jsonStrings[Path.GetFileName(jsonFile)] = System.Text.Encoding.UTF8.GetString(bytes);
                    }
                    
                    // Process all .jpg files
                    List<string> jpgFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.jpg");
                    allJpgFiles.AddRange(jpgFiles);
                    List<string> sortedJpgFiles = jpgFiles.OrderBy(Path.GetFileName).ToList();
                    foreach (string jpgFile in sortedJpgFiles)
                    {
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jpgFile);

                        string currentDirectory = Path.GetDirectoryName(jpgFile);
                        directoryPath = Path.GetDirectoryName(jpgFile);
                        Debug.Log("Current Directory of " + Path.GetFileName(jpgFile) + ": " + currentDirectory);
                    }
                    
                    // Process all .jpeg files
                    List<string> jpegFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.jpeg");
                    List<string> sortedJpegFiles = jpegFiles.OrderBy(Path.GetFileName).ToList();
                    foreach (string jpegFile in sortedJpegFiles)
                    {
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jpegFile);

                        string currentDirectory = Path.GetDirectoryName(jpegFile);
                        directoryPath = Path.GetDirectoryName(jpegFile);
                        Debug.Log("Current Directory of " + Path.GetFileName(jpegFile) + ": " + currentDirectory);
                    }
                }
                var ordered = jsonStrings.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                jsonStrings = ordered;

                foreach (KeyValuePair<string, string> entry in jsonStrings)
                {
                    Debug.Log("Key: " + entry.Key);
                }

                string lastJsonValue = jsonStrings.LastOrDefault().Value;
                string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[i]));
                FileBrowserHelpers.CopyFile(FileBrowser.Result[i], destinationPath);
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