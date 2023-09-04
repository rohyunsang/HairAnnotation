using UnityEngine;
using UnityEngine.UI;

public class ButtonZoom : MonoBehaviour
{
    public RawImage mainImage; // ū �̹���
    public float zoomFactor = 2.0f; // Ȯ�� ����
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = mainImage.rectTransform.localScale; // ���� ������ ����
    }

    public void OnButtonClick(RectTransform buttonRectTransform)
    {
        // ��ư ������ �߽����� �̹����� Ȯ���մϴ�.
        Vector3 zoomedScale = new Vector3(zoomFactor, zoomFactor, 1.0f);
        mainImage.rectTransform.localScale = zoomedScale;

        // ��ư �߽����� �̹��� ��ġ ����
        Vector2 offset = buttonRectTransform.anchoredPosition * (zoomFactor - 1);
        mainImage.rectTransform.anchoredPosition = -offset;
    }

    public void ResetZoom()
    {
        mainImage.rectTransform.localScale = originalScale;
        mainImage.rectTransform.anchoredPosition = Vector2.zero;
    }
}