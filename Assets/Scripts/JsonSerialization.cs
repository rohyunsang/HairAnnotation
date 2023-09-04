using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class PimpleEntry
{
    public string name;
    public List<int> points = new List<int>();
}

[System.Serializable]
public class PimpleData
{
    public List<PimpleEntry> _F = new List<PimpleEntry>();
    public List<PimpleEntry> _R30 = new List<PimpleEntry>();
    public List<PimpleEntry> _L30 = new List<PimpleEntry>();
    public void Clear()
    {
        _F.Clear();
        _R30.Clear();
        _L30.Clear();
    }
}
[System.Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
}

[System.Serializable]
public class ScoreData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}
[System.Serializable]
public class RectangleEntry
{
    public string name;
    public List<int> points = new List<int>();
}

[System.Serializable]
public class ImageData
{
    public string imageName;
    public List<RectangleEntry> rectangleEntries = new List<RectangleEntry>();
}

[System.Serializable]
public class RectangleData
{
    public string imageName;
    public List<RectangleEntry> rectangleEntries = new List<RectangleEntry>();
}

[System.Serializable]
public class SerializableDict
{
    public string userName = "";
    public PimpleData pimpleData;
    public List<ScoreData> scoreDatas = new List<ScoreData>();
    public List<ImageData> imageDataList = new List<ImageData>();
}

[System.Serializable]
public class SerializablePimpleDict
{
    public string userName = "";
    public PimpleData pimpleData;
    
}


public class JsonSerialization : MonoBehaviour
{
    public GameObject jsonParsingObj;
    public GameObject parentPortraits;
    public GameObject loginManagerObj;


    private const float PIXEL_WIDTH = 2136f;
    private const float PIXEL_HEIGHT = 3216f;
    private const float PIXEL_FACEIMAGE_WIDTH = 715f;
    private const float PIXEL_FACEIMAGE_HEIGHT = 1080f;

    private Dictionary<string, List<RectangleEntry>> rectangleDict = new Dictionary<string, List<RectangleEntry>>();

    public GameObject saveCompleteImage;

    public GameObject FileBrowserObj;

    public Text[] scores;

    public GameObject faceImage_f;
    public GameObject faceImage_l30;
    public GameObject faceImage_r30;


    public void InitializeRectangleDict()
    {
        int totalEntries = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count;

        for (int idx = 0; idx < totalEntries; idx++)
        {
            GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];
            string currentId = jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id;

            if (!rectangleDict.ContainsKey(currentId))
            {
                rectangleDict[currentId] = new List<RectangleEntry>();
            }

            foreach (GameObject child in gameObjectList.gameObjects)
            {
                // ... (기존의 사각형 항목을 추가/수정하는 로직)
                RectTransform rectTransform = child.GetComponent<RectTransform>();

                Vector2 pivot = rectTransform.pivot;
                Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * rectTransform.sizeDelta.x, (0.5f - pivot.y) * rectTransform.sizeDelta.y);
                Vector2 adjustedPosition = rectTransform.anchoredPosition + pivotOffset;

                Vector2 center = adjustedPosition + new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2);
                Vector2 topLeft = new Vector2(center.x - rectTransform.sizeDelta.x / 2, center.y + rectTransform.sizeDelta.y / 2);
                Vector2 bottomRight = new Vector2(center.x + rectTransform.sizeDelta.x / 2, center.y - rectTransform.sizeDelta.y / 2);

                int originalX1 = (int)(topLeft.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
                int originalY1 = (int)((PIXEL_FACEIMAGE_HEIGHT - topLeft.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);
                int originalX2 = (int)(bottomRight.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
                int originalY2 = (int)((PIXEL_FACEIMAGE_HEIGHT - bottomRight.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);

                RectangleEntry entry = new RectangleEntry();
                entry.name = child.name;

                entry.points.Add(originalX1);
                entry.points.Add(originalY1);
                entry.points.Add(originalX2);
                entry.points.Add(originalY2);

                // Check if an entry with the same name exists
                RectangleEntry existingEntry = rectangleDict[currentId].Find(e => e.name == entry.name);

                if (existingEntry != null)
                {
                    // Overwrite the points for the existing entry
                    existingEntry.points = entry.points;
                }
                else
                {
                    // Add the new entry if it doesn't exist
                    rectangleDict[currentId].Add(entry);
                }
            }
        }
    }

   


    public void SaveBtn()
    {
        int idx = jsonParsingObj.GetComponent<JsonParsing>().idx;
        GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];
        string currentId = jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id;

        if (!rectangleDict.ContainsKey(currentId))
        {
            rectangleDict[currentId] = new List<RectangleEntry>();
        }

        foreach (GameObject child in gameObjectList.gameObjects)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();

            Vector2 pivot = rectTransform.pivot;
            Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * rectTransform.sizeDelta.x, (0.5f - pivot.y) * rectTransform.sizeDelta.y);
            Vector2 adjustedPosition = rectTransform.anchoredPosition + pivotOffset;

            Vector2 center = adjustedPosition + new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2);
            Vector2 topLeft = new Vector2(center.x - rectTransform.sizeDelta.x / 2, center.y + rectTransform.sizeDelta.y / 2);
            Vector2 bottomRight = new Vector2(center.x + rectTransform.sizeDelta.x / 2, center.y - rectTransform.sizeDelta.y / 2);

            int originalX1 = (int)(topLeft.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            int originalY1 = (int)((PIXEL_FACEIMAGE_HEIGHT - topLeft.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);
            int originalX2 = (int)(bottomRight.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            int originalY2 = (int)((PIXEL_FACEIMAGE_HEIGHT - bottomRight.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);

            RectangleEntry entry = new RectangleEntry();
            entry.name = child.name;

            entry.points.Add(originalX1);
            entry.points.Add(originalY1);
            entry.points.Add(originalX2);
            entry.points.Add(originalY2);

            // Check if an entry with the same name exists
            RectangleEntry existingEntry = rectangleDict[currentId].Find(e => e.name == entry.name);

            if (existingEntry != null)
            {
                // Overwrite the points for the existing entry
                existingEntry.points = entry.points;
            }
            else
            {
                // Add the new entry if it doesn't exist
                rectangleDict[currentId].Add(entry);
            }
        }

       
    }
    public void SaveJson()
    {
        SerializableDict serializableDict = new SerializableDict
        {
            userName = loginManagerObj.GetComponent<SaveUserData>().idField.text
        };

        foreach (var kvp in rectangleDict)
        {
            ImageData entry = new ImageData
            {
                imageName = kvp.Key,
                rectangleEntries = kvp.Value
            };
            serializableDict.imageDataList.Add(entry);
        }

        // Populate scores
        ScoreData scoreData = new ScoreData();
        foreach (Text scoreText in scores)
        {
            ScoreEntry entry = new ScoreEntry
            {
                name = scoreText.name,
                score = int.Parse(scoreText.text)
            };
            scoreData.scores.Add(entry);
        }

        serializableDict.scoreDatas.Add(scoreData);

        PimpleData pimpleData = new PimpleData();
        PopulatePimpleDataFrom(faceImage_f, pimpleData);
        PopulatePimpleDataFrom(faceImage_l30, pimpleData);
        PopulatePimpleDataFrom(faceImage_r30, pimpleData);
        serializableDict.pimpleData = pimpleData;


        string json = JsonUtility.ToJson(serializableDict, true);
        string currentPath = FileBrowserObj.GetComponent<FileBrowserTest>().filePath;

        // 'jsons' 디렉토리 경로를 생성합니다.
        string jsonsDirectoryPath = Path.Combine(currentPath, "jsons");
        Directory.CreateDirectory(jsonsDirectoryPath);  // 디렉토리가 없으면 생성하고, 있으면 아무것도 하지 않습니다.

        // 'jsons' 디렉토리 안에 .json 파일을 저장합니다.
        string jsonFilePath = Path.Combine(jsonsDirectoryPath, "anno" + "_" + System.DateTime.Now.ToString("MM_dd_HH_mm_ss") + ".json");
        File.WriteAllText(jsonFilePath, json);

        Debug.Log("Complete");
    }

    void TransformPosition(Transform child, out int x, out int y)
    {
        RectTransform rectTransform = child.GetComponent<RectTransform>();

        Vector2 pivot = rectTransform.pivot;
        Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * rectTransform.sizeDelta.x, (0.5f - pivot.y) * rectTransform.sizeDelta.y);
        Vector2 adjustedPosition = rectTransform.anchoredPosition + pivotOffset;

        Vector2 center = adjustedPosition + new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2);

        x = (int)(center.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
        y = (int)((PIXEL_FACEIMAGE_HEIGHT - center.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);
    }

    void PopulatePimpleDataFrom(GameObject go, PimpleData pimpleData)
    {
        List<PimpleEntry> targetList = null;

        switch (go.name)
        {
            case "faceImage_f":
                targetList = pimpleData._F;
                break;
            case "faceImage_r30":
                targetList = pimpleData._R30;
                break;
            case "faceImage_l30":
                targetList = pimpleData._L30;
                break;
            default:
                Debug.LogWarning("Unknown GameObject name: " + go.name);
                return;
        }

        foreach (Transform child in go.transform)
        {
            if (child.gameObject.tag == "Circle")
            {
                int x, y;
                TransformPosition(child, out x, out y);

                PimpleEntry entry = new PimpleEntry();
                entry.name = child.gameObject.name;
                entry.points.Add(x);
                entry.points.Add(y);

                targetList.Add(entry);
            }
        }
    }
    public void SavePimpleAndUserName()
    {
        SerializablePimpleDict serializableDict = new SerializablePimpleDict
        {
            userName = loginManagerObj.GetComponent<SaveUserData>().idField.text
        };

        PimpleData pimpleData = new PimpleData();
        PopulatePimpleDataFrom(faceImage_f, pimpleData);
        PopulatePimpleDataFrom(faceImage_l30, pimpleData);
        PopulatePimpleDataFrom(faceImage_r30, pimpleData);
        serializableDict.pimpleData = pimpleData;

        string json = JsonUtility.ToJson(serializableDict, true);
        string currentPath = FileBrowserObj.GetComponent<FileBrowserTest>().filePath;

        // Create the 'jsons' directory path.
        string jsonsDirectoryPath = Path.Combine(currentPath, "pimples");
        Directory.CreateDirectory(jsonsDirectoryPath);  // Create the directory if it doesn't exist, otherwise do nothing.

        // Save the .json file inside the 'jsons' directory.
        string jsonFilePath = Path.Combine(jsonsDirectoryPath, "pimple" + ".json");
        File.WriteAllText(jsonFilePath, json);

        Debug.Log("Pimple and userName save complete.");
    }


    public void OffSaveCompleteImage()
    {
        saveCompleteImage.SetActive(false);
    }
}
