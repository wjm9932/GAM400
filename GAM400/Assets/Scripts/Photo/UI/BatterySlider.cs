using UnityEngine;
using UnityEngine.UI;

namespace Paparazzi
{
    public class BatterySlider : MonoBehaviour
    {
        [SerializeField] private MagicPhotoPlayer photo;
        [SerializeField] private Slider slider;

        private float max = 100.0f;

        void Start()
        {
            slider.value = photo.Battery / max;
        }

        void Update()
        {
            slider.value = photo.Battery / max;
        }
    }
}
