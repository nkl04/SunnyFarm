using UnityEngine;

public class UIBoundedConstrant : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void Update()
    {
        KeepWithinBounds();
    }

    private void KeepWithinBounds()
    {
        Vector3[] worldCorners = new Vector3[4];

        rectTransform.GetWorldCorners(worldCorners);

        Vector3[] canvasCorners = new Vector3[4];

        canvasRectTransform.GetWorldCorners(canvasCorners);

        Vector3 newPosition = rectTransform.position;

        if (worldCorners[0].x < canvasCorners[0].x)
        {
            float offset = canvasCorners[0].x - worldCorners[0].x;

            newPosition.x += offset;

            rectTransform.position = newPosition;
        }
    }
}
