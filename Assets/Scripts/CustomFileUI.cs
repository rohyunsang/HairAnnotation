using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        Dictionary<string, byte[]> innerDict = fileBrowserObj.GetComponent<FileBrowserTest>().jpgDict[parts[0]];
        byte[] imageData = innerDict[parts[1]];
        // byte[]로부터 Texture2D 생성하기
        Texture2D texture = new Texture2D(2, 2);  // 초기 크기는 어떤 값을 사용해도 됩니다.
        texture.LoadImage(imageData);  // byte[]를 사용하여 텍스처 로드

        // Texture2D를 RawImage의 텍스처로 설정하기
        rawImage.texture = texture;
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
