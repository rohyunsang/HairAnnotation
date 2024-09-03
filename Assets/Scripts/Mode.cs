using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mode : MonoBehaviour, IPointerClickHandler
{
    public GameObject circlePrefab;
    public RawImage rawImage;
    // 생성된 circlePrefab들을 저장하는 리스트
    private List<GameObject> instantiatedCircles = new List<GameObject>();

    public void OnPointerClick(PointerEventData eventData) // mouse click instant circle
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Check if mouse pointer is over the RawImage (이미 이 메서드가 호출되었으므로 이 조건은 생략해도 됩니다.)
            if (RectTransformUtility.RectangleContainsScreenPoint(rawImage.rectTransform, eventData.position, eventData.pressEventCamera))
            {
                // Instantiate circlePrefab at the mouse position
                // Vector2 localPoint;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
                //GameObject newCircle = Instantiate(circlePrefab, rawImage.rectTransform.TransformPoint(localPoint), Quaternion.identity, rawImage.transform);

                // 생성된 circlePrefab을 리스트에 추가
                //instantiatedCircles.Add(newCircle);
            }
        }
    }

}
