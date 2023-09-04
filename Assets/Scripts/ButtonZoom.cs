using UnityEngine;
using UnityEngine.UI;

public class ButtonZoom : MonoBehaviour
{
    public RawImage mainImage; // 큰 이미지
    public float zoomFactor = 2.0f; // 확대 배율
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = mainImage.rectTransform.localScale; // 원래 스케일 저장
    }

    public void OnButtonClick(RectTransform buttonRectTransform)
    {
        // 버튼 영역을 중심으로 이미지를 확대합니다.
        Vector3 zoomedScale = new Vector3(zoomFactor, zoomFactor, 1.0f);
        mainImage.rectTransform.localScale = zoomedScale;

        // 버튼 중심으로 이미지 위치 조정
        Vector2 offset = buttonRectTransform.anchoredPosition * (zoomFactor - 1);
        mainImage.rectTransform.anchoredPosition = -offset;
    }

    public void ResetZoom()
    {
        mainImage.rectTransform.localScale = originalScale;
        mainImage.rectTransform.anchoredPosition = Vector2.zero;
    }
}