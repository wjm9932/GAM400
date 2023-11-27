using UnityEngine;
using UnityEngine.UI;

namespace Paparazzi
{
    public class ImagePlacedView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private RawImage image;

        public PhotoData SelectedData { get; private set; }

        public void Open(PhotoData data)
        {
            SelectedData = data;
            panel.SetActive(true);
            image.texture = data.Image;

            SelectedData.Holder.transform.rotation = Quaternion.identity;
        }

        public void Close()
        {
            panel.SetActive(false);
        }

        private void Start()
        {
            SelectedData = null;
            panel.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {

            }
            else if (Input.GetKey(KeyCode.E))
            {

            }
        }
    }
}
