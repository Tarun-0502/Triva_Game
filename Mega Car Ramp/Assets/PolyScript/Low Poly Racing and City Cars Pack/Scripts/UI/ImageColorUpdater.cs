using UnityEngine;
using UnityEngine.UI;

namespace PolyScript
{
    [RequireComponent(typeof(Image))]
    public class ImageColorUpdater : MonoBehaviour
    {
        private Image image;

        private void OnEnable() => image = GetComponent<Image>();
        public void UpdateColor(Component sender, object data)
        {
            if (data is Color)
                image.color = (Color)data;
        }
    }
}
