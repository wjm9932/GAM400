using UnityEngine;
using UnityEngine.UI;

namespace Paparazzi
{
    public class PhotoAlbum : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        [SerializeField] private MagicPhotoPlayer player;
        [SerializeField] private GameObject panel;
        [SerializeField] private RawImage photo;
        [SerializeField] private Button button;

        private PhotoData data;
        
        public void Set(PhotoData data)
        {
            this.data = data;
            IsActive = true;
            photo.texture = data.Image;
            panel.SetActive(true);
        }

        public void Reset()
        {
            data = null;
            IsActive = true;
            photo.texture = null;
            panel.SetActive(false);
        }

        void Start()
        {
            IsActive = false;
            button.onClick.AddListener(SelectImage);
        }

        private void SelectImage()
        {
            if (data == null)
            {
                Debug.LogError("Should be set Photo Data");
                return;
            }

            player.SelectedImage(data);
            Reset();
        }
    }
}
