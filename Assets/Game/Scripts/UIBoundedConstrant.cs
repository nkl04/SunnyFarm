using UnityEngine;

public class UIBoundedConstraint : MonoBehaviour
{
    private RectTransform parentRectTransform;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    void Update()
    {
        ConstrainPosition();


    }

    void ConstrainPosition()
    {
        Vector3[] corners = GetWorldCorners(rectTransform);

        Vector3[] screenCorners = GetScreenCorners();

        Debug.Log(screenCorners[0].y + " " + corners[0].y);
    }

    Vector3[] GetWorldCorners(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];

        rectTransform.GetWorldCorners(corners);

        return corners;
    }

    Vector3[] GetScreenCorners()
    {
        Vector3[] corners = new Vector3[4];

        // Get the camera (assuming the main camera)
        Camera mainCamera = Camera.main;

        // Get screen corners in world space
        corners[0] = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)); // Bottom Left
        corners[1] = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, mainCamera.nearClipPlane)); // Top Left
        corners[2] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane)); // Top Right
        corners[3] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, mainCamera.nearClipPlane)); // Bottom Right

        return corners;
    }
}
