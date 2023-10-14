using UnityEngine;
using UnityEngine.UI;

namespace Paparazzi
{
    public class CaptureView : MonoBehaviour
    {
        public bool CanCaptureNow { get; private set; }
        [SerializeField] private GameObject panel;
        [SerializeField] private Image captureEffect;

        private float waitTime = 1.0f;
        private float timer;

        public void Open()
        {
            panel.SetActive(true);
        }

        public void Close()
        {
            panel.SetActive(false);
        }

        public void Captured()
        {
            CanCaptureNow = false;
            timer = 0.0f;

            captureEffect.gameObject.SetActive(true);
            captureEffect.color = Color.yellow;
        }

        private void Start()
        {
            CanCaptureNow = true;
            timer = 0.0f;

            panel.SetActive(false);
            captureEffect.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!CanCaptureNow)
            {
                timer += Time.deltaTime;
                if (timer < waitTime)
                {
                    var alphaValue = timer / waitTime;
                    var color = captureEffect.color;
                    color.a = (1.0f - alphaValue);
                    captureEffect.color = color;
                }
                else
                {
                    CanCaptureNow = true;
                    captureEffect.gameObject.SetActive(false);
                }
            }
        }
    }
}
