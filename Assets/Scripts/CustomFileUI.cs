using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomFileUI : MonoBehaviour
{
    public GameObject fileBrowserObj;
    public GameObject scrollViewMain;
    public GameObject namePrefab;
    public List<GameObject> buttons = new List<GameObject>();

    public RawImage rawImage;


    public void ButtonVisibleControl(GameObject clickedButton)
    {
        // 클릭된 버튼의 이름 가져오기
        string clickedButtonName = clickedButton.name;


        // 모든 버튼을 순회하면서
        foreach (GameObject button in buttons)
        {
            if (button.name.Contains(clickedButtonName))
            {
                if (button.Equals(clickedButton)) continue;
                // 현재 활성화 상태에 따라 버튼의 상태를 토글한다.
                button.SetActive(!button.activeSelf);
            }
        }
    }

    public void ShowIamgeButton(GameObject clickedButton)
    {
        // clickedButton의 이름을 가져온다.
        // buttons에서
        string s = clickedButton.name;
        string[] parts = s.Split('_');

        if (parts.Length > 1)
        {
            FindObjectOfType<App>().currentFolderName = parts[0];
            FindObjectOfType<App>().currentImageName = parts[1];
        }

        Dictionary<string, byte[]> innerDict = fileBrowserObj.GetComponent<FileBrowserTest>().jpgDict[parts[0]];
        byte[] imageData = innerDict[parts[1]];
        // byte[]로부터 Texture2D 생성하기
        Texture2D texture = new Texture2D(2, 2);  // 초기 크기는 어떤 값을 사용해도 됩니다.
        texture.LoadImage(imageData);  // byte[]를 사용하여 텍스처 로드

        // Texture2D를 RawImage의 텍스처로 설정하기
        rawImage.texture = texture;

        InitRawImage();


        foreach (KeyValuePair<string, byte[]> entry in FindObjectOfType<FileBrowserTest>().jsonDict)
        {
            string fileName = entry.Key;
            byte[] fileBytes = entry.Value;

            // 파일 이름과 byte 배열의 길이를 출력
            Debug.Log("File Name: " + fileName + ", Byte Length: " + fileBytes.Length);
        }

        if (FindObjectOfType<FileBrowserTest>().jsonDict.ContainsKey(parts[1] + ".json"))
        {
            ParsingJson(FindObjectOfType<FileBrowserTest>().jsonDict[parts[1] + ".json"]);
        }
    }

    private void ParsingJson(byte[] jsonBytes)
    {
        // byte[] 데이터를 문자열로 변환
        string jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes);

        // JSON을 C# 객체로 파싱
        CircleGroupsWithAverage parsedData = JsonUtility.FromJson<CircleGroupsWithAverage>(jsonString);

        // 파싱된 데이터를 사용해 circleGroups 업데이트
        if (parsedData != null)
        {
            Debug.Log("Average Thickness: " + parsedData.averageThickness);

            // ManualCircle 인스턴스 가져오기
            ManualCircle manualCircle = FindObjectOfType<ManualCircle>();

            // 기존 데이터를 비우고 새 데이터를 넣습니다.
            manualCircle.circleGroups.Clear();
            manualCircle.circleGroups = parsedData.groups;
            manualCircle.circles.Clear();
            manualCircle.circleCount = 0;

            foreach (var group in parsedData.groups)
            {
                Debug.Log("Group Name: " + group.name + ", Thickness: " + group.thickness);
                foreach (var circle in group.circles)
                {
                    GameObject newCircle = null;
                    if (manualCircle.circleCount % 3 == 0)
                    {
                        newCircle = Instantiate(manualCircle.circleNamePrefab, manualCircle._rawImage.transform);
                        string circleName = "id : " + (manualCircle.circleCount / 3 + 1);  // 이름 설정
                        newCircle.transform.GetChild(0).GetComponent<Text>().text = circleName;
                    }
                    else
                    {
                        newCircle = Instantiate(manualCircle.circlePrefab, manualCircle._rawImage.transform);
                    }

                    RectTransform rectTransform = newCircle.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(circle.x, circle.y);

                    manualCircle.circles.Add(newCircle);
                    manualCircle.circleCount++;

                }
            }

            Debug.Log("총 " + manualCircle.circleCount + "개의 원이 생성되었습니다.");
        }
        else
        {
            Debug.LogError("Failed to parse JSON.");
        }
    }


    private void InitRawImage()
    {
        FindObjectOfType<ManualCircle>().circles.Clear();
        FindObjectOfType<ManualCircle>().circleGroups.Clear();
        FindObjectOfType<ManualCircle>().circleCount = 0;

        // rawImage의 모든 자식 오브젝트 삭제
        foreach (Transform child in rawImage.transform)
        {
            Destroy(child.gameObject);
        }
    }


    public void ScrollViewInstantiate(Dictionary<string, Dictionary<string, byte[]>> jpgDict)
    {
        var orderedDict = jpgDict.OrderBy(entry => entry.Key);
        // For each directory entry in the dictionary
        foreach (var directoryEntry in orderedDict)
        {
            string directoryName = directoryEntry.Key;
            var imageFilesDict = directoryEntry.Value;
            // Instantiate a new button prefab
            GameObject newDirectoryButton = Instantiate(namePrefab, scrollViewMain.transform);
            newDirectoryButton.name = directoryName;

            // Set the button's text to the image name
            newDirectoryButton.GetComponentInChildren<Text>().text = directoryName;
            newDirectoryButton.GetComponent<Button>().onClick.AddListener(() => ButtonVisibleControl(newDirectoryButton));  // 이벤트 추가
            buttons.Add(newDirectoryButton);

            // For each image file in the directory
            foreach (var imageEntry in imageFilesDict)
            {
                string imageName = imageEntry.Key;
                byte[] imageData = imageEntry.Value;

                // Instantiate a new button prefab
                GameObject newButton = Instantiate(namePrefab, scrollViewMain.transform);
                newButton.name = directoryName + "_" + imageName;
                newButton.gameObject.SetActive(false);

                // Set the button's text to the image name
                newButton.GetComponentInChildren<Text>().text = imageName;
                newButton.GetComponent<Button>().onClick.AddListener(() => ShowIamgeButton(newButton));
                buttons.Add(newButton);
            }
        }
    }
}
