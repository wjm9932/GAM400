using UnityEngine;

namespace Paparazzi
{
    public enum PhotoMode
    {
        None = 0, Capture, ImagePlace, Album
    }

    public class MagicPhotoPlayer : MonoBehaviour
    {
        public AudioClip Open_Clip;
        public AudioClip Close_Clip;
        public AudioClip Capture_Clip;
        public int MaxCaptureCount { get; private set; }
        public int RemainCaptureCount => MaxCaptureCount - currentCapturedCount;
        public PhotoAlbumData PhotoAlbumData { get; private set; }

        [SerializeField] private MagicCamera magicCamera;
        [SerializeField] private MagicCameraUIManager uiManager;

        private int currentCapturedCount;
        private PhotoMode mode;

        public void SetCaptureMaxCount(int maxCount)
        {
            MaxCaptureCount = maxCount;
            currentCapturedCount = 0;
            magicCamera.ResetHolders(maxCount);
        }

        public void SelectedImage(PhotoData data)
        {
            mode = PhotoMode.ImagePlace;
            uiManager.ClosePhotoAlbumView();
            uiManager.OpenImagePlacedView(data);
        }

        void Start()
        {
            mode = PhotoMode.None;

            PhotoAlbumData = new PhotoAlbumData();
        }

        void Update()
        {
            switch (mode)
            {
                case PhotoMode.None:
                    UpdateNoneMode();
                    break;

                case PhotoMode.Capture:
                    UpdateCaptureMode();
                    break;

                case PhotoMode.ImagePlace:
                    UpdateImagePlacedMode();
                    break;

                case PhotoMode.Album:
                    UpdateAlbumMode();
                    break;

                default:
                    Debug.LogWarning("Not made Mode");
                    break;
            }
        }

        private void UpdateNoneMode()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (RemainCaptureCount > 0)
                {
                    SoundManager.instance.SFXPlay("Open", Open_Clip);
                    uiManager.OpenCaptureView();
                    mode = PhotoMode.Capture;
                }
                else
                {
                    //TODO: Active Warning Panel;
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                uiManager.OpenPhotoAlbumView(PhotoAlbumData.Albumes);
                mode = PhotoMode.Album;
            }
        }

        private void UpdateCaptureMode()
        {
            if (!uiManager.CanCapture())
                return;

            if (Input.GetMouseButtonDown(1))
            {
                SoundManager.instance.SFXPlay("Capture", Capture_Clip);
                uiManager.Capture();
                var result = magicCamera.GetCaptureResult();
                PhotoAlbumData.Add(result);
            }

            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
            {
                SoundManager.instance.SFXPlay("Close", Close_Clip);
                uiManager.CloseCaptureView();
                mode = PhotoMode.None;
            }
        }

        private void UpdateImagePlacedMode()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var photoData = uiManager.GetCurrentPlacedData();
                magicCamera.UseImage(photoData);
                PhotoAlbumData.Remove(photoData.Holder);
                uiManager.CloseImagePlacedView();
                mode = PhotoMode.None;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiManager.CloseImagePlacedView();
                mode = PhotoMode.None;
            }
        }

        private void UpdateAlbumMode()
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                mode = PhotoMode.None;
                uiManager.ClosePhotoAlbumView();
            }
        }
    }
}
