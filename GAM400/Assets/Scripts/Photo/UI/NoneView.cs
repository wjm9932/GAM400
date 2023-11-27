using TMPro;
using UnityEngine;

namespace Paparazzi
{
    public class NoneView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private MagicPhotoPlayer photo;
        [SerializeField] private TMP_Text albumCountText;

        public void Open()
        {
            panel.gameObject.SetActive(true);
        }

        public void Close()
        {
            panel.gameObject.SetActive(false);
        }

        void Awake()
        {
            panel.gameObject.SetActive(false);

            albumCountText.text = string.Format("X {0}", 0);
        }

        void Update()
        {
            if (!panel.gameObject.activeSelf)
                return;

            albumCountText.text = string.Format("X {0}", photo.PhotoAlbumData.Albumes.Count);
        }
    }
}
