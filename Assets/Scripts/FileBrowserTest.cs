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
                    Debug.Log("Selected a directory: " + FileBrowser.Result[i]);
                    FindObjectOfType<App>().directoryPath = FileBrowser.Result[i];

                    // Process all .jpg files
                    List<string> jpgFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.jpg");  // get all jpgs
                    Debug.Log(jpgFiles.Count + " .jpg files found in the directory.");
                    //allJpgFileNames.AddRange(jpgFiles); // add name
                    List<string> sortedJpgFiles = jpgFiles.OrderBy(Path.GetFileName).ToList();  //sorting
                    foreach (string jpgFile in sortedJpgFiles)
                    {
                        byte[] fileBytes = FileBrowserHelpers.ReadBytesFromFile(jpgFile);
                        string fileName = Path.GetFileName(jpgFile);
                        string currentDirectory = Path.GetDirectoryName(jpgFile);
                        string currentDirectoryName = Path.GetFileName(currentDirectory);

                        string combinedFileName = Path.GetFileName(currentDirectory) + "_" + fileName; // Combine directory name and file name
                        
                        if (!jpgDict.ContainsKey(currentDirectoryName))
                        {
                            jpgDict[currentDirectoryName] = new Dictionary<string, byte[]>();
                        }
                        jpgDict[currentDirectoryName].Add(fileName,fileBytes);

                        allJpgFileNames.Add(combinedFileName);
                        allJpgFiles.Add(fileBytes);
                        directoryPath = Path.GetDirectoryName(jpgFile);
                    }

                    CustomFileUIObj.GetComponent<CustomFileUI>().ScrollViewInstantiate(jpgDict);

                    string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[i]));
                    FileBrowserHelpers.CopyFile(FileBrowser.Result[i], destinationPath);
                }
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