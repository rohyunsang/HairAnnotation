using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObjInstantManager : MonoBehaviour
{
    public GameObject rectanglePrefab;
    public GameObject circlePrefab;
    public RawImage spawnPoint_f;
    public RawImage spawnPoint_l30;
    public RawImage spawnPoint_r30;
    public RawImage spawnPoint_empty;

    public GameObject JsonParsingObj;


    private const float PIXEL_WIDTH = 2136f;
    private const float PIXEL_HEIGHT = 3216f;

    private const float PIXEL_FACEIMAGE_WIDTH = 715f;
    private const float PIXEL_FACEIMAGE_HEIGHT = 1080f;

    


    public void PimpleInstant(PimpleData pimpleData)
    {
        // Instantiate pimples for _F
        if (pimpleData._F != null && pimpleData._F.Any())
        {
            InstantiatePimples(pimpleData._F, spawnPoint_f);
        }

        // Instantiate pimples for _R30
        if (pimpleData._R30 != null && pimpleData._R30.Any())
        {
            InstantiatePimples(pimpleData._R30, spawnPoint_r30);
        }

        // Instantiate pimples for _L30
        if (pimpleData._L30 != null && pimpleData._L30.Any())
        {
            InstantiatePimples(pimpleData._L30, spawnPoint_r30);
        }
    }

    private void InstantiatePimples(List<PimpleEntry> pimpleEntries, RawImage spawnPoint)
    {
        foreach (var pimple in pimpleEntries)
        {
            // Convert points to screen space
            float x = pimple.points[0] / PIXEL_WIDTH * PIXEL_FACEIMAGE_WIDTH;
            float y = (PIXEL_HEIGHT - pimple.points[1]) / PIXEL_HEIGHT * PIXEL_FACEIMAGE_HEIGHT;

            // Instantiate a pimple object from the prefab
            GameObject pimpleObj = Instantiate(circlePrefab, spawnPoint.transform);

            // Set the parent of the pimple object
            pimpleObj.transform.SetParent(spawnPoint.transform, false);

            // Set the position of the pimple object
            RectTransform rectTransform = pimpleObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(x - PIXEL_FACEIMAGE_WIDTH / 2, y - PIXEL_FACEIMAGE_HEIGHT / 2);  // Adjust for the pivot in the center

            // Set the name of the pimple object
            pimpleObj.name = pimple.name;
            pimpleObj.layer = LayerMask.NameToLayer("UI");
        }
    }

    public void RectangleInstant(List<Info> parsedInfo)
    {
        parsedInfo = parsedInfo.OrderBy(info => info.id).ToList();
        foreach (Info info in parsedInfo)
        {
            RawImage currentSpawnPoint = spawnPoint_empty; // Default spawn point
            if (info.id.Contains("_L30"))
            {
                currentSpawnPoint = spawnPoint_l30;
            }
            else if (info.id.Contains("_R30"))
            {
                currentSpawnPoint = spawnPoint_r30;
            }
            else if (info.id.Contains("_F"))
            {
                if (info.id.Contains("_Fb") || info.id.Contains("Ft"))
                {

                }
                else
                {
                    currentSpawnPoint = spawnPoint_f;
                }
            }

            GameObjectList gameObjectList = new GameObjectList();

            List<GameObject> newRectangles = new List<GameObject>();
            for (int i = 0; i < info.region_name.Length; i++)
            {
                int x1 = info.point[i * 4];
                int y1 = info.point[i * 4 + 1];
                int x2 = info.point[i * 4 + 2];
                int y2 = info.point[i * 4 + 3];

                // Instantiate a rectangle object from the prefab
                GameObject rectangle = Instantiate(rectanglePrefab, currentSpawnPoint.transform);

                // Set the parent of the rectangle object
                rectangle.transform.SetParent(currentSpawnPoint.transform, false);

                // Set the size of the rectangle object with scaling
                RectTransform rectTransform = rectangle.GetComponent<RectTransform>();

                // The width and height of the rectangle
                float rectWidth = (x2 - x1) / PIXEL_WIDTH * PIXEL_FACEIMAGE_WIDTH;
                float rectHeight = (y2 - y1) / PIXEL_HEIGHT * PIXEL_FACEIMAGE_HEIGHT;

                // The position of the center of the rectangle
                Vector2 rectCenter = new Vector2(((x1 + x2) / 2) / PIXEL_WIDTH * PIXEL_FACEIMAGE_WIDTH, (3216 - (y1 + y2) / 2) / PIXEL_WIDTH * PIXEL_FACEIMAGE_WIDTH);

                // Set the size and position of the rectangle
                rectTransform.sizeDelta = new Vector2(rectWidth, rectHeight);
                rectTransform.anchoredPosition = rectCenter - new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2); // here is your offset
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                // Set the name of the rectangle object
                rectangle.gameObject.name = info.region_name[i];
                rectangle.layer = LayerMask.NameToLayer("UI");

                Text regionNameText = rectangle.GetComponentInChildren<Text>();
                if (regionNameText != null)
                {
                    regionNameText.text = info.region_name[i];
                }
                newRectangles.Add(rectangle);
                gameObjectList.gameObjects = newRectangles;
            }
            JsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Add(gameObjectList);
        }
    }
}
