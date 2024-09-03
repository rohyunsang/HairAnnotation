using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using UnityEngine;
public class CircleMoving : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private UILineConnector lineConnector;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        lineConnector = FindObjectOfType<UILineConnector>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta * 0.2f;

        // float adjustedPosX = newPos.x - rectTransform.rect.width * rectTransform.pivot.x;
        // float adjustedPosY = newPos.y - rectTransform.rect.height * rectTransform.pivot.y;

        rectTransform.anchoredPosition = newPos;
    }
}