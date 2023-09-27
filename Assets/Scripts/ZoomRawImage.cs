using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZoomRawImage : MonoBehaviour, IDragHandler, IScrollHandler
{
    public RawImage rawImage;
    public GameObject circlePrefab;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private RectTransform rectTransform;
    private Vector3 initialScale;
    private float zoomSpeed = 0.1f;
    private float maxZoom = 5.0f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // 현재 위치의 RectTransform
        initialScale = transform.localScale;  // 현재 Local Scale 저장
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle) // 마우스 중앙 버튼 확인
        {
            rectTransform.anchoredPosition += eventData.delta * 2; // 드래그 처리
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        var desiredScale = transform.localScale + delta;
        desiredScale = ClampDesiredScale(desiredScale);
        transform.localScale = desiredScale;
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initialScale, desiredScale);
        desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
        return desiredScale;
    }
}