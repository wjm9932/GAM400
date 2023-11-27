using UnityEngine;
using UnityEngine.UI;

namespace Paparazzi
{
    public class ImagePlacedView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject albumPanel;
        [SerializeField] private RawImage image;
        [SerializeField] private MagicCamera magicCam;

        private bool needToUpdate = false;
        public PhotoData SelectedData { get; private set; }

        public void Open(PhotoData data)
        {
            needToUpdate = true;
            SelectedData = data;
            panel.SetActive(true);
            image.texture = data.Image;

            albumPanel.gameObject.transform.rotation = Quaternion.identity;
        }

        public void Close()
        {
            needToUpdate = false;
            panel.SetActive(false);
        }

        private void Start()
        {
            needToUpdate = false;
            SelectedData = null;
            panel.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var angle = albumPanel.gameObject.transform.rotation.eulerAngles.z;
                angle += 15;
                SelectedData.Holder.transform.localEulerAngles = new Vector3(0, 0, angle);
                albumPanel.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                var angle = albumPanel.gameObject.transform.rotation.eulerAngles.z;
                angle -= 15;
                SelectedData.Holder.transform.localEulerAngles = new Vector3(0, 0, angle);
                albumPanel.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
