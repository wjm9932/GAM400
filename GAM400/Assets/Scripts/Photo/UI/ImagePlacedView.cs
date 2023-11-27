using UnityEngine;
using UnityEngine.UI;

namespace Paparazzi
{
    public class ImagePlacedView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject albumPanel;
        [SerializeField] private RawImage image;

        private bool needToUpdate = false;
        public PhotoData SelectedData { get; private set; }

        public void Open(PhotoData data)
        {
            needToUpdate = true;
            SelectedData = data;
            panel.SetActive(true);
            image.texture = data.Image;

            albumPanel.gameObject.transform.rotation = Quaternion.identity;
            //SelectedData.Holder.transform.rotation = Quaternion.identity;
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
            //if (!needToUpdate)
            //    return;

            //var currentRotation = albumPanel.gameObject.transform.rotation.eulerAngles.z;
            //Quaternion newRotation = Quaternion.Euler(0, 0, currentRotation);

            //if (Input.GetKeyDown(KeyCode.Q))
            //    newRotation = Quaternion.Euler(0, 0, currentRotation + 15);
            //else if (Input.GetKeyDown(KeyCode.E))
            //    newRotation = Quaternion.Euler(0, 0, currentRotation - 15);

            //albumPanel.gameObject.transform.rotation = newRotation;
            //SelectedData.Holder.transform.RotateAround(SelectedData.Holder.transform.position, Vector3.up, 20 * Time.deltaTime);
            //SelectedData.Holder.gameObject.transform.localEulerAngles = newRotation.eulerAngles;
        }
    }
}
