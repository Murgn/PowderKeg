using UnityEngine;

/// <summary>
/// Small helper class to convert viewport, screen or world positions to canvas space.
/// Only works with screen space canvases.
/// </summary>
/// <example>
/// <code>
/// objectOnCanvasRectTransform.anchoredPosition = specificCanvas.WorldToCanvasPoint(worldspaceTransform.position);
/// </code>
/// </example>
namespace Murgn.Utils
{
    public static class RectTransformPositioningExtensions
    {
        public static Vector3 WorldToCanvasPosition(this RectTransform rectTransform, Vector3 worldPosition, Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            var viewportPosition = camera.WorldToViewportPoint(worldPosition);
            return rectTransform.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ScreenToCanvasPosition(this RectTransform rectTransform, Vector3 screenPosition)
        {
            var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                screenPosition.y / Screen.height,
                0);
            return rectTransform.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ViewportToCanvasPosition(this RectTransform rectTransform, Vector3 viewportPosition)
        {
            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
            var scale = rectTransform.sizeDelta;
            return Vector3.Scale(centerBasedViewPortPosition, scale);
        }
    }
}