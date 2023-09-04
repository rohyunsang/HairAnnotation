using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RectangleEntryFile
{
    public string name;
    public List<int> points;
}

[System.Serializable]
public class ImageDataFile
{
    public string imageName;
    public List<RectangleEntry> rectangleEntries;
}

[System.Serializable]
public class RootObject
{
    public List<ImageData> imageDataList;
    public PimpleData pimpleData;
    public List<ScoreData> scoreDatas;
}

[System.Serializable]
public class Info  //structure
{
    public string id;  // needs int to string 
    public string[] region_name;
    public List<int> point;
}

[System.Serializable]
public class GameObjectList
{
    public List<GameObject> gameObjects = new List<GameObject>();
}

public class JsonParsing : MonoBehaviour
{
    public GameObject PanelManagerObj;
    public GameObject ObjInstantGameObject; // using call ObjInstantManager Class Function
    public RawImage faceImage_f;
    public RawImage faceImage_l30;
    public RawImage faceImage_r30;
    public RawImage faceImage_empty;

    public GameObject WorkEndImage;

    [SerializeField]
    public List<GameObjectList> jsonSquares = new List<GameObjectList>();
    public List<Texture2D> imageDatas = new List<Texture2D>();
    public List<Info> parsedInfo = new List<Info>();

    public int idx = 0;

    public GameObject portraitPrefab;
    public Transform scrollView;
    public Transform scrollViewInitPanel;

    public GameObject failWindow;
    public GameObject CropManagerObj;
    public GameObject StatusObj;

    // Private variable to remember if the squares are active
    private bool areSquaresActive = true;

    [SerializeField]
    PimpleData pimpleData;

    [SerializeField]
    List<ScoreData> scoreDatas;

    public void InitData()
    {
        InitPimple();
        InitStatusScore();
    }
    public void InitStatusScore()
    {
        if (scoreDatas != null)
            scoreDatas.Clear();
    }
    public void InitPimple()
    {
        if (pimpleData != null)
            pimpleData.Clear();
    }
    public void DataPassToCrop()
    {
        CropFaceImage cropScript = CropManagerObj.GetComponent<CropFaceImage>();
        cropScript.imageDatas = this.imageDatas;
        cropScript.parsedInfo = this.parsedInfo;
    }

    public void MakeJsonArray(string jsonData)
    {
        ParseJSONData(jsonData);
    }
    public void MakeImageStringArray(byte[] bytes)
    {
        // Create a Texture2D from the image bytes
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        imageDatas.Add(texture);
    }
    public void CheckingFileCount()
    {
        if (jsonSquares.Count != 0)
        {
            InitPortrait();
        }
        else
        {
            failWindow.SetActive(true);
            Invoke("FailWindowSetActiveFalse", 3f);
            ClearObjs();
        }
    }

    public void ClearObjs()
    {
        jsonSquares.Clear();
        imageDatas.Clear();
        parsedInfo.Clear();
        foreach (Transform child in faceImage_f.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in faceImage_l30.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in faceImage_r30.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in faceImage_empty.transform)
        {
            Destroy(child.gameObject);
        }
    }



    

    public void OnOffBox()
    {
        if (areSquaresActive)
        {
            // Deactivate all game objects in jsonSquares
            foreach (GameObjectList gameObjectList in jsonSquares)
            {
                foreach (GameObject obj in gameObjectList.gameObjects)
                {
                    obj.SetActive(false);
                }
            }
        }
        else
        {
            // Activate all game objects in jsonSquares
            foreach (GameObjectList gameObjectList in jsonSquares)
            {
                foreach (GameObject obj in gameObjectList.gameObjects)
                {
                    obj.SetActive(true);
                }
            }
        }

        // Toggle the state
        areSquaresActive = !areSquaresActive;
    }

    public void SetChildRectScale()
    {
        foreach (GameObjectList gameObjectList in jsonSquares)
        {
            foreach (GameObject obj in gameObjectList.gameObjects)
            {
                // Get the RectTransform component of the parent GameObject
                RectTransform parentRect = obj.GetComponent<RectTransform>();

                // Make sure the parent has a RectTransform component
                if (parentRect != null)
                {
                    // Try to find a child GameObject named "ChildRect"
                    Transform childTransform = obj.transform.Find("ChildRect");

                    if (childTransform != null)
                    {
                        // Get the RectTransform component of the child GameObject
                        RectTransform childRect = childTransform.GetComponent<RectTransform>();

                        if (childRect != null)
                        {
                            // Set the child's width and height to match the parent's
                            childRect.sizeDelta = parentRect.sizeDelta + new Vector2(10f, 10f);
                        }
                    }
                }
            }
        }
    }

    private void FailWindowSetActiveFalse()
    {
        failWindow.SetActive(false);
    }
    public void Portrait()
    {
        faceImage_f.texture = imageDatas[0];
        faceImage_l30.texture = imageDatas[4];
        faceImage_r30.texture = imageDatas[6];

        faceImage_r30.gameObject.SetActive(false);
        faceImage_l30.gameObject.SetActive(false);
        faceImage_f.gameObject.SetActive(false);

        GameObject portraitInstanceA = Instantiate(portraitPrefab, scrollView.transform);

        portraitInstanceA.name = parsedInfo[0].id;
        portraitInstanceA.GetComponent<Image>().sprite = Sprite.Create(imageDatas[0], new Rect(0, 0, imageDatas[0].width, imageDatas[0].height), Vector2.one * 0.5f);

        GameObject portraitInstanceB = Instantiate(portraitPrefab, scrollView.transform);

        portraitInstanceB.name = parsedInfo[4].id;
        portraitInstanceB.GetComponent<Image>().sprite = Sprite.Create(imageDatas[4], new Rect(0, 0, imageDatas[4].width, imageDatas[4].height), Vector2.one * 0.5f);

        GameObject portraitInstanceC = Instantiate(portraitPrefab, scrollView.transform);

        portraitInstanceC.name = parsedInfo[6].id;
        portraitInstanceC.GetComponent<Image>().sprite = Sprite.Create(imageDatas[6], new Rect(0, 0, imageDatas[6].width, imageDatas[6].height), Vector2.one * 0.5f);

    }
    public void InitPortrait()
    {
        Debug.Log("InitPortrait");
        for (int i = 0; i < imageDatas.Count; i++)
        {
            GameObject portraitInstanceB = Instantiate(portraitPrefab, scrollViewInitPanel.transform);
            portraitInstanceB.name = i.ToString();
            portraitInstanceB.GetComponent<Image>().sprite = Sprite.Create(imageDatas[i], new Rect(0, 0, imageDatas[i].width, imageDatas[i].height), Vector2.one * 0.5f);
            Destroy(portraitInstanceB.GetComponent<Button>());
        }
    }



    public void QueueManager(string portraitName) // using btn;
    {
        SetChildRectScale();  // 사각형 크기 조절 

        faceImage_r30.gameObject.SetActive(false);
        faceImage_l30.gameObject.SetActive(false);
        faceImage_f.gameObject.SetActive(false);

        if(portraitName.Contains("R30")) 
            faceImage_r30.gameObject.SetActive(true);
        else if (portraitName.Contains("L30")) 
            faceImage_l30.gameObject.SetActive(true);
        else 
            faceImage_f.gameObject.SetActive(true);

    }
    public void ParseJSONData(string jsonData)
    {
        var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonData);

        // Parse image data
        foreach (var imageData in rootObject.imageDataList)
        {
            Info imageInfo = new Info();
            imageInfo.id = imageData.imageName;
            imageInfo.region_name = new string[imageData.rectangleEntries.Count];
            imageInfo.point = new List<int>();

            int i = 0;  // region_name 배열 인덱싱을 위한 변수

            foreach (var rectangleEntry in imageData.rectangleEntries)
            {
                imageInfo.region_name[i] = rectangleEntry.name;
                imageInfo.point.AddRange(rectangleEntry.points);
                i++;
            }

            parsedInfo.Add(imageInfo);
        }

        // Parse pimple data
        pimpleData = rootObject.pimpleData;
        // You can process the pimpleData as needed here

        // Parse score data
        scoreDatas = rootObject.scoreDatas;
        // You can process the scoreDatas as needed here

        ObjInstantGameObject.GetComponent<ObjInstantManager>().RectangleInstant(parsedInfo); // parsing한 data를 전달 
        if(pimpleData != null)
            ObjInstantGameObject.GetComponent<ObjInstantManager>().PimpleInstant(pimpleData);
        if(scoreDatas != null)
            StatusObj.GetComponent<Status>().LoadStatusText(scoreDatas);
    }
}
