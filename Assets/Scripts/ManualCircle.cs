using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class CirclePosition
{
    public float x;
    public float y;

    public CirclePosition(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

[System.Serializable]
public class CircleGroup
{
    public string name;
    public float thickness;
    public List<CirclePosition> circles;

    public CircleGroup(string name, float thickness, List<CirclePosition> circles)
    {
        this.name = name;
        this.thickness = thickness;
        this.circles = circles;
    }
}
[System.Serializable]
public class CircleGroupsWithAverage
{
    public float averageThickness;
    public List<CircleGroup> groups;

    public CircleGroupsWithAverage(float averageThickness, List<CircleGroup> groups)
    {
        this.averageThickness = averageThickness;
        this.groups = groups;
    }
}

public class ManualCircle : MonoBehaviour, IPointerClickHandler
{
    public RawImage _rawImage;
    public GameObject circleNamePrefab;
    public GameObject circlePrefab;
    public List<GameObject> circles = new List<GameObject>();
    [SerializeField]
    public List<CircleGroup> circleGroups = new List<CircleGroup>();
    public ManualJson manualJson;
    public int circleCount = 0;
    public List<float> thicknesses = new List<float>();

    public void Start()
    {
        manualJson = FindObjectOfType<ManualJson>();
    }

    public void SaveCircleGroups()
    {
        // 새로운 구조로 그룹 생성
        List<CircleGroup> newCircleGroups = new List<CircleGroup>();

        foreach (var group in circleGroups)
        {
            if (group.circles.Count == 3)
            {
                string groupName = "id : " + (newCircleGroups.Count + 1);

                // 두 번째와 세 번째 원 사이의 거리 계산
                float thickness = Vector2.Distance(
                    new Vector2(group.circles[1].x, group.circles[1].y),
                    new Vector2(group.circles[2].x, group.circles[2].y)
                );

                thickness *= 5f; // rawImage 1200 * 800, Origin Image 6000 * 4000

                // thickness 값을 리스트에 추가
                thicknesses.Add(thickness);

                // 그룹을 새로운 구조로 저장
                CircleGroup newGroup = new CircleGroup(groupName, thickness, group.circles);
                newCircleGroups.Add(newGroup);
            }
        }

        // thickness의 평균값 계산
        float averageThickness = thicknesses.Count > 0 ? thicknesses.Average() : 0f;

        // CircleGroupsWithAverage 객체 생성
        CircleGroupsWithAverage circleGroupsWithAverage = new CircleGroupsWithAverage(averageThickness, newCircleGroups);

        // JSON 변환
        string json = JsonUtility.ToJson(circleGroupsWithAverage, true);

        // SaveData 메서드를 통해 JSON 저장
        manualJson.SaveData(json);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (RectTransformUtility.RectangleContainsScreenPoint(_rawImage.rectTransform, eventData.position, eventData.pressEventCamera))
        {
            circleCount++;

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rawImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                GameObject newCircle = null;
                CirclePosition circlePosition;

                if ((circleCount - 1) % 3 == 0)  // 첫 번째 원
                {
                    newCircle = Instantiate(circleNamePrefab, _rawImage.rectTransform.TransformPoint(localPoint), Quaternion.identity, _rawImage.transform);
                    string circleName = "id : " + (circleCount / 3 + 1);  // 이름 설정
                    newCircle.transform.GetChild(0).GetComponent<Text>().text = circleName;
                    circlePosition = new CirclePosition(localPoint.x, localPoint.y);
                }
                else
                {
                    newCircle = Instantiate(circlePrefab, _rawImage.rectTransform.TransformPoint(localPoint), Quaternion.identity, _rawImage.transform);
                    circlePosition = new CirclePosition(localPoint.x, localPoint.y);
                }

                circles.Add(newCircle);

                if (circleGroups.Count == 0 || circleGroups[circleGroups.Count - 1].circles.Count == 3)
                {
                    circleGroups.Add(new CircleGroup("", 0f, new List<CirclePosition>()));
                }

                circleGroups[circleGroups.Count - 1].circles.Add(circlePosition);

                if (circleGroups[circleGroups.Count - 1].circles.Count == 3)
                {
                    Debug.Log("새로운 3개의 원 그룹이 생성되었습니다.");
                    SaveCircleGroups();
                }
            }
        }
    }
}