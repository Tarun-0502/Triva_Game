using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolyScript
{
    public class ColorTextureGenerator : MonoBehaviour
    {

        [field: SerializeField] public TextureType TextureType { get; private set; } = TextureType.Gradient;

        [SerializeField] int textureWidth = 256;
        [SerializeField] int textureHeight = 256;

        private Image imageComponent;
        private Texture2D generatedTexture;

        void Awake()
        {
            imageComponent = GetComponent<Image>();
            GenerateTexture();
        }

        public void GenerateTexture()
        {
            generatedTexture = new Texture2D(textureWidth, textureHeight);
            generatedTexture.wrapMode = TextureWrapMode.Clamp;
            generatedTexture.filterMode = FilterMode.Bilinear;

            if (TextureType == TextureType.Gradient)
                GenerateGradientTexture();
            else if (TextureType == TextureType.Wheel)
                GenerateWheelTexture();

            generatedTexture.Apply();

            // Create a sprite from the texture and assign it to the Image component
            Rect rect = new Rect(0, 0, textureWidth, textureHeight);
            Sprite sprite = Sprite.Create(generatedTexture, rect, new Vector2(0.5f, 0.5f));
            imageComponent.sprite = sprite;
        }

        private void GenerateGradientTexture()
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float saturation = (float)y / (textureHeight - 1);

                for (int x = 0; x < textureWidth; x++)
                {
                    float hue = (float)x / (textureWidth - 1);

                    Color color = Color.HSVToRGB(hue, saturation, 1f);
                    generatedTexture.SetPixel(x, y, color);
                }
            }
        }

        private void GenerateWheelTexture()
        {
            Vector2 center = new Vector2(textureWidth / 2f, textureHeight / 2f);
            float maxRadius = textureWidth / 2f;

            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    Vector2 position = new Vector2(x, y);
                    Vector2 direction = position - center;
                    float radius = direction.magnitude / maxRadius;
                    float angle = Mathf.Atan2(direction.y, direction.x);
                    if (angle < 0) angle += 2 * Mathf.PI;

                    if (radius <= 1f)
                    {
                        float hue = angle / (2 * Mathf.PI);
                        float saturation = radius;
                        Color color = Color.HSVToRGB(hue, saturation, 1f);
                        generatedTexture.SetPixel(x, y, color);
                    }
                    else
                    {
                        generatedTexture.SetPixel(x, y, Color.clear);
                    }
                }
            }
        }

        public Texture2D GetTexture()
        {
            return generatedTexture;
        }
    }
}
