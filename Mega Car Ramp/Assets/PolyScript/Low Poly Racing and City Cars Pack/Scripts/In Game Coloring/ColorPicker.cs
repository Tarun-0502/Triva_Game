using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PolyScript
{
    public class ColorPicker : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField] RectTransform cursor;                      // Reference to the cursor
        [SerializeField] GameEvent onColorChanged;          // Event to fire when color changes
        private Texture2D colorTexture;                   // Texture of the color spectrum
        private RectTransform rectTransform;              // RectTransform of the color spectrum
        private ColorTextureGenerator textureGenerator;   // Reference to the Texture Generator
        public Color CurrentColor { get; private set; }
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            textureGenerator = GetComponent<ColorTextureGenerator>();
            colorTexture = textureGenerator.GetTexture();
            CurrentColor = Color.white;
            onColorChanged?.Raise(CurrentColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateCursorAndColor(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateCursorAndColor(eventData);
        }

        private void UpdateCursorAndColor(PointerEventData eventData)
        {
            Vector2 localPoint;

            // Convert the screen point to a local point within the RectTransform of the color picker
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);

            // Correctly determine the center of the circle within the RectTransform
            Vector2 center = rectTransform.rect.center;

            // Handle based on texture type
            if (textureGenerator.TextureType == TextureType.Wheel)
            {
                // For Wheel (circular) texture
                Vector2 direction = localPoint - center;
                float radius = rectTransform.rect.width / 2f;
                float distanceFromCenter = direction.magnitude;

                // Clamp to circle
                if (distanceFromCenter > radius)
                {
                    direction = direction.normalized * radius;
                    localPoint = center + direction;
                }
            }
            else if (textureGenerator.TextureType == TextureType.Gradient)
            {
                // For Gradient (square) texture
                // Clamp the localPoint to the bounds of the rectTransform's rect
                float xMin = rectTransform.rect.xMin;
                float xMax = rectTransform.rect.xMax;
                float yMin = rectTransform.rect.yMin;
                float yMax = rectTransform.rect.yMax;

                localPoint.x = Mathf.Clamp(localPoint.x, xMin, xMax);
                localPoint.y = Mathf.Clamp(localPoint.y, yMin, yMax);
            }

            // Move the cursor to the new clamped local position
            cursor.localPosition = localPoint;

            // Get the color at the cursor's (possibly clamped) position
            CurrentColor = GetColorFromTexture(localPoint);



            // Fire the color changed event
            onColorChanged?.Raise(CurrentColor);
        }

        private Color GetColorFromTexture(Vector2 localPoint)
        {
            if (textureGenerator.TextureType == TextureType.Gradient)
            {
                // Normalize the coordinates to [0,1]
                float normalizedX = Mathf.InverseLerp(rectTransform.rect.xMin, rectTransform.rect.xMax, localPoint.x);
                float normalizedY = Mathf.InverseLerp(rectTransform.rect.yMin, rectTransform.rect.yMax, localPoint.y);

                // Get the color from the texture
                return colorTexture.GetPixelBilinear(normalizedX, normalizedY);
            }
            else if (textureGenerator.TextureType == TextureType.Wheel)
            {
                // Calculate hue and saturation based on polar coordinates
                Vector2 center = rectTransform.rect.center;
                Vector2 direction = localPoint - center;
                float radius = rectTransform.rect.width / 2f;
                float distanceFromCenter = direction.magnitude / radius;
                float angle = Mathf.Atan2(direction.y, direction.x);
                if (angle < 0) angle += 2 * Mathf.PI;

                if (distanceFromCenter <= 1f)
                {
                    float hue = angle / (2 * Mathf.PI);
                    float saturation = distanceFromCenter;
                    return Color.HSVToRGB(hue, saturation, 1f);
                }
                else
                {
                    return Color.clear;
                }
            }
            else
            {
                return Color.clear;
            }
        }
        public void OnCarChanged()
        {
            onColorChanged?.Raise(CurrentColor);
        }
    }
}
