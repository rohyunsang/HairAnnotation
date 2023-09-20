using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using System.Collections.Generic;

public class FileBrowserTest : MonoBehaviour
{
    public string filePath = "";
    public GameObject AdjsustImageObj;

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
                    filePath = Path.GetDirectoryName(jpgFile);
                    Debug.Log("Current Directory of " + Path.GetFileName(jpgFile) + ": " + currentDirectory);
                }
            }


            AdjsustImageObj.GetComponent<AdjustImage>().InitRawImage();
        }
    }
}