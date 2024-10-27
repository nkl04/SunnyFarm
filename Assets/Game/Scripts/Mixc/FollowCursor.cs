using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (rectTransform == null || canvas == null) return;

        Vector2 mousePosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
        mousePosition,
            canvas.worldCamera,
            out Vector2 localPoint);

        Vector2 adjustedPosition = localPoint + new Vector2(rectTransform.rect.width, -rectTransform.rect.height);

        rectTransform.anchoredPosition = adjustedPosition;
    }
}
