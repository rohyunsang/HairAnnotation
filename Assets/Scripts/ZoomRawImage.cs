using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZoomRawImage : MonoBehaviour, IPointerClickHandler
{
    public RawImage rawImage;
    public GameObject circlePrefab;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    float zoomLevel = 0.5f;
    Vector2 zoomCenter = new Vector2(0.5f, 0.5f);

    // 확대/축소의 속도를 조절하는 변수입니다.
    public float zoomSpeed = 0.5f;
    // 확대/축소의 최대/최소 범위를 정의하는 변수입니다.
    public float minZoomLevel = 0.1f;
    public float maxZoomLevel = 1f;

    // 마우스 드래그를 위한 변수들
    private bool isDragging = false;
    private Vector2 startDragPosition;
    private Vector2 startZoomCenter;

    const float rawImageWidth = 1200f;
    const float rawImageHeight = 800f;

    void Update()
    {
        HandleZoom();
        HandleDrag();
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            zoomLevel -= scrollInput * zoomSpeed;
            zoomLevel = Mathf.Clamp(zoomLevel, minZoomLevel, maxZoomLevel);
            ApplyZoom();
        }
    }

    void HandleDrag()
    {
        // 마우스 휠 버튼이 눌렸을 때
        if (Input.GetMouseButtonDown(2))
        {
            isDragging = true;
            startDragPosition = Input.mousePosition;
            startZoomCenter = zoomCenter;
        }

        // 마우스 휠 버튼이 떼어졌을 때
        if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }

        // 드래그 중일 때
        if (isDragging)
        {
            Vector2 dragDelta = (Vector2)Input.mousePosition - startDragPosition;

            // 화면 크기에 따라 이동량을 조절합니다.
            float widthRatio = rawImage.texture.width / rawImageWidth;
            float heightRatio = rawImage.texture.height / rawImageHeight;
            float dragFactor = zoomLevel / (rawImageWidth * widthRatio);
            Vector2 movement = dragDelta * dragFactor;

            zoomCenter = startZoomCenter - movement;

            // zoomCenter 값을 유효한 범위 내로 제한합니다.
            zoomCenter.x = Mathf.Clamp(zoomCenter.x, 0.5f * zoomLevel, 1 - 0.5f * zoomLevel);
            zoomCenter.y = Mathf.Clamp(zoomCenter.y, 0.5f * zoomLevel, 1 - 0.5f * zoomLevel);

            // RawImage의 이동과 동일하게 오브젝트들을 이동합니다.
            foreach (GameObject obj in spawnedObjects)
            {
                obj.transform.localPosition += new Vector3(movement.x * rawImageWidth, movement.y * rawImageHeight, 0);
            }

            ApplyZoom();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 왼쪽 버튼이 클릭됐는지 확인합니다.
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                // 로컬 포인트를 UV 좌표로 변환합니다.
                Rect currentUVRect = rawImage.uvRect;
                Vector2 uvCoord = new Vector2(
                    (localPoint.x / rawImage.rectTransform.rect.width + 0.5f) * currentUVRect.width + currentUVRect.x,
                    (localPoint.y / rawImage.rectTransform.rect.height + 0.5f) * currentUVRect.height + currentUVRect.y
                );
                Vector2 imageSize = new Vector2(rawImageWidth, rawImageHeight);
                Vector2 clickedPositionInImage = new Vector2(uvCoord.x * imageSize.x, uvCoord.y * imageSize.y);

                // 실제 게임 월드에서의 위치를 계산
                Vector3 spawnPosition = rawImage.rectTransform.TransformPoint(localPoint);

                // 오브젝트 생성
                GameObject spawnedObject = Instantiate(circlePrefab, spawnPosition, Quaternion.identity, rawImage.transform);

                // 생성된 오브젝트의 참조를 리스트에 추가
                spawnedObjects.Add(spawnedObject);
            }
        }
    }

    void ApplyZoom()
    {
        Rect newUVRect = new Rect(
            zoomCenter.x - (zoomLevel / 2f),
            zoomCenter.y - (zoomLevel / 2f),
            zoomLevel,
            zoomLevel
        );

        rawImage.uvRect = newUVRect;
    }
}