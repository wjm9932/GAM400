using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class MagicCameraUIManager : MonoBehaviour
    {
        [SerializeField] private CaptureView captureViewPanel;
        [SerializeField] private ImagePlacedView imagePlacedPanel;
        [SerializeField] private PhotoAlbumView photoAlbumPanel;

        public void OpenCaptureView()
        {
            captureViewPanel.Open();
        }

        public void OpenImagePlacedView(PhotoData data)
        {
            imagePlacedPanel.Open(data);
        }

        public void OpenPhotoAlbumView(List<PhotoData> albums)
        {
            photoAlbumPanel.Open(albums);
        }

        public void CloseCaptureView()
        {
            captureViewPanel.Close();
        }

        public void CloseImagePlacedView()
        {
            imagePlacedPanel.Close();
        }

        public void ClosePhotoAlbumView()
        {
            photoAlbumPanel.Close();
        }

        public PhotoData GetCurrentPlacedData()
        {
            return imagePlacedPanel.SelectedData;
        }

        public bool CanCapture()
        {
            return captureViewPanel.CanCaptureNow;
        }

        public void Capture()
        {
            captureViewPanel.Captured();
        }
    }
}
