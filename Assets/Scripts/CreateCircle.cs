using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
public class CreateCircle : MonoBehaviour, IPointerClickHandler  // Attach RawImage in AdjustingPanel
{
    public RawImage adjustRawImage;
    public GameObject circlePrefab;
    public GameObject[] circles;
    public GameObject UILineRendererObj;
    public GameObject AdjustingImageObj;
    

    public void CalculateCircleLength() // using CheckBtn in DotListImage in AdjustingPanel
    {
        if (circles.Length < 4)
        {
            Debug.LogError("Not enough circles to calculate length!");
        }
        float[] lengths = new float[4];
        lengths[0] = Vector2.Distance(circles[0].GetComponent<RectTransform>().anchoredPosition, circles[1].GetComponent<RectTransform>().anchoredPosition);
        lengths[1] = Vector2.Distance(circles[1].GetComponent<RectTransform>().anchoredPosition, circles[2].GetComponent<RectTransform>().anchoredPosition);
        lengths[2] = Vector2.Distance(circles[2].GetComponent<RectTransform>().anchoredPosition, circles[3].GetComponent<RectTransform>().anchoredPosition);
        lengths[3] = Vector2.Distance(circles[3].GetComponent<RectTransform>().anchoredPosition, circles[0].GetComponent<RectTransform>().anchoredPosition);

        AdjustingImageObj.GetComponent<AdjustImage>().circleLengths = lengths;
        AdjustingImageObj.GetComponent<AdjustImage>().circleLength = (lengths[0] + lengths[0] + lengths[0] + lengths[0])/4;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (circles.Length >= 4) return;
        // Check if mouse pointer is over the RawImage (이미 이 메서드가 호출되었으므로 이 조건은 생략해도 됩니다.)
        if (RectTransformUtility.RectangleContainsScreenPoint(adjustRawImage.rectTransform, eventData.position, eventData.pressEventCamera))
        {
            // Instantiate circlePrefab at the mouse position
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(adjustRawImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
            GameObject newCircle = Instantiate(circlePrefab, adjustRawImage.rectTransform.TransformPoint(localPoint), Quaternion.identity, adjustRawImage.transform);
            System.Array.Resize(ref circles, circles.Length + 1);
            circles[circles.Length - 1] = newCircle;
        }
        if (circles.Length == 4)
        {
            RectTransform[] circleTransforms = new RectTransform[circles.Length];
            for (int i = 0; i < circles.Length; i++)
            {
                circleTransforms[i] = circles[i].GetComponent<RectTransform>();
            }
            UILineRendererObj.GetComponent<UILineConnector>().transforms = circleTransforms;
        }
    }
}