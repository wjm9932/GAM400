using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class PhotoAlbumView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private PhotoAlbum photoPrefab;
        [SerializeField] private GameObject leftPage;
        [SerializeField] private GameObject rightPage;

        private List<PhotoAlbum> leftPageAlbums;
        private List<PhotoAlbum> rightPageAlbums;

        private int maxCountInPage = 8;

        public void Open(List<PhotoData> albums)
        {
            panel.SetActive(true);

            var length = albums.Count;
            if (length > maxCountInPage)
                length = maxCountInPage;

            var halfSize = maxCountInPage / 2;
            for (int i = 0; i < length; ++i)
            {
                var album = GetAlbum(i, halfSize);
                album.Set(albums[i]);
            }

            if (length < maxCountInPage)
            {
                for (int i = length; i < maxCountInPage; ++i)
                {
                    var album = GetAlbum(i, halfSize);
                    album.Reset();
                }
            }
        }

        public void Close()
        {
            panel.SetActive(false);
        }

        void Start()
        {
            photoPrefab.gameObject.SetActive(false);
            leftPageAlbums = new List<PhotoAlbum>();
            rightPageAlbums = new List<PhotoAlbum>();

            var halfSize = maxCountInPage / 2;
            for (int i = 0; i < halfSize; ++i)
            {
                var leftPhoto = Instantiate(photoPrefab, leftPage.transform);
                var rightPhoto = Instantiate(photoPrefab, rightPage.transform);

                leftPageAlbums.Add(leftPhoto);
                rightPageAlbums.Add(rightPhoto);

                leftPhoto.gameObject.SetActive(false);
                rightPhoto.gameObject.SetActive(false);
            }
        }

        private PhotoAlbum GetAlbum(int index, int halfSize)
        {
            if (index < halfSize)
                return leftPageAlbums[index % halfSize];
            else
                return rightPageAlbums[index % halfSize];
        }
    }
}
