using System;
using System.Collections;
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
        public AudioClip Hold_Clip;
        public AudioClip Capture_Clip;

        //public AudioClip Paste_Clip;
        //public AudioClip SelectPic_Clip;
        //public AudioClip Fail_Clip;

        public float Battery { get; private set; }
        public PhotoAlbumData PhotoAlbumData { get; private set; }
        public int maxCount { get; private set; }

        [SerializeField] private PlayerMovement playerMove;
        [SerializeField] private MagicCamera magicCamera;
        [SerializeField] private MagicCameraUIManager uiManager;
        [SerializeField] private GameObject[] models;

        private bool canTakePicture => Battery > 12.5f && PhotoAlbumData.Albumes.Count < 24;
        private bool needUpdateBattery;
        private PhotoMode mode;

        public void SelectedImage(PhotoData data)
        {
            //SoundManager.instance.SFXPlay("SelectPic", SelectPic_Clip);
            UpdateCharacterModel(false);
            mode = PhotoMode.ImagePlace;
            uiManager.ClosePhotoAlbumView();
            uiManager.OpenImagePlacedView(data);
        }

        void Start()
        {
            mode = PhotoMode.None;
            uiManager.OpenNoneView();
            Battery = 100.0f;
            maxCount = 24;
            needUpdateBattery = false;

            PhotoAlbumData = new PhotoAlbumData();
            magicCamera.ResetHolders(maxCount);
        }

        void Update()
        {
            if (PauseMenu.isPaused)
                return;

            UpdateBattery();

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

            playerMove.stopMove = (mode == PhotoMode.Album);
        }

        private void UpdateNoneMode()
        {
            if (Cursor.lockState != CursorLockMode.Locked || Cursor.visible != false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                SoundManager.instance.SFXPlay("Hold", Hold_Clip);
                uiManager.OpenCaptureView();
                uiManager.CloseNoneView();
                mode = PhotoMode.Capture;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SoundManager.instance.SFXPlay("Open", Open_Clip);
                uiManager.OpenPhotoAlbumView(PhotoAlbumData.Albumes);
                uiManager.CloseNoneView();
                mode = PhotoMode.Album;
            }
        }

        private void UpdateCaptureMode()
        {
            if (!uiManager.CanCapture())
                return;

            if (Cursor.lockState != CursorLockMode.Locked || Cursor.visible != false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonDown(1) && canTakePicture)
            {
                SoundManager.instance.SFXPlay("Capture", Capture_Clip);
                uiManager.Capture();
                var result = magicCamera.GetCaptureResult();
                PhotoAlbumData.Add(result);

                StopAllCoroutines();
                StartCoroutine(UpdateBatteryFlag());
                Battery -= 20.0f;

                if (Battery < 0.0f)
                    Battery = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                SoundManager.instance.SFXPlay("UnHold", Hold_Clip);
                uiManager.CloseCaptureView();
                mode = PhotoMode.None;
                uiManager.OpenNoneView();
            }
        }

        private void UpdateImagePlacedMode()
        {
            if (Cursor.lockState != CursorLockMode.Locked || Cursor.visible != false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                //SoundManager.instance.SFXPlay("Paste", Paste_Clip);
                var photoData = uiManager.GetCurrentPlacedData();
                magicCamera.UseImage(photoData);
                PhotoAlbumData.Remove(photoData.Holder);
                uiManager.CloseImagePlacedView();
                mode = PhotoMode.None;
                uiManager.OpenNoneView();
                UpdateCharacterModel(true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                SoundManager.instance.SFXPlay("UnHold", Hold_Clip);
                uiManager.CloseImagePlacedView();
                mode = PhotoMode.None;
                uiManager.OpenNoneView();
                UpdateCharacterModel(true);
            }
        }

        private void UpdateAlbumMode()
        {
            if (Cursor.lockState != CursorLockMode.None || Cursor.visible != true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SoundManager.instance.SFXPlay("Close", Close_Clip);
                mode = PhotoMode.None;
                uiManager.ClosePhotoAlbumView();
                uiManager.OpenNoneView();
            }
        }

        private void UpdateCharacterModel(bool isActive)
        {
            foreach (var model in models)
                model.gameObject.SetActive(isActive);
        }

        private void UpdateBattery()
        {
            if (Battery >= 100.0f)
                return;

            if (!needUpdateBattery)
            {
                //SoundManager.instance.SFXPlay("Fail", Fail_Clip);
                return;
            }

            Battery += 2.5f * Time.deltaTime;
        }

        private IEnumerator UpdateBatteryFlag()
        {
            needUpdateBattery = false;
            yield return new WaitForSeconds(5.0f);
            needUpdateBattery = true;
        }
    }
}
