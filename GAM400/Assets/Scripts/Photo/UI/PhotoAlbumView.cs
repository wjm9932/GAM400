using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paparazzi
{
    public class PhotoAlbumView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private PhotoAlbum photoPrefab;
        [SerializeField] private GameObject leftPage;
        [SerializeField] private GameObject rightPage;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        private List<PhotoData> allData;
        private List<PhotoAlbum> leftPageAlbums;
        private List<PhotoAlbum> rightPageAlbums;

        private int maxCountInPage = 8;
        private int currentPage = 1;
        private int maxPage = 3;
        private int halfSize => maxCountInPage / 2;

        public void Open(List<PhotoData> albums)
        {
            allData = albums;

            currentPage = 1;
            panel.SetActive(true);
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(allData.Count > currentPage * maxCountInPage);

            var length = albums.Count;
            if (length > maxCountInPage)
                length = maxCountInPage;

            for (int i = 0; i < length; ++i)
            {
                var album = GetAlbum(i);
                album.Set(albums[i]);
            }

            if (length < maxCountInPage)
            {
                for (int i = length; i < maxCountInPage; ++i)
                {
                    var album = GetAlbum(i);
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

            leftButton.onClick.AddListener(() => UpdatePage(false));
            rightButton.onClick.AddListener(() => UpdatePage(true));

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

        private PhotoAlbum GetAlbum(int index)
        {
            if (index < halfSize)
                return leftPageAlbums[index % halfSize];
            else
                return rightPageAlbums[index % halfSize];
        }

        private void UpdatePage(bool isNext)
        {
            int offset = isNext ? 1 : -1;
            currentPage += offset;

            leftButton.gameObject.SetActive(currentPage != 1);
            rightButton.gameObject.SetActive(currentPage != maxPage && allData.Count > currentPage * maxCountInPage);

            // Reset All
            for (int i = 0; i < halfSize; ++i)
            {
                leftPageAlbums[i].Reset();
                rightPageAlbums[i].Reset();
            }

            int index = currentPage - 1;
            var count = allData.Count - maxCountInPage * index;
            for (int i = 0; i < count; ++i)
            {
                var album = GetAlbum(i);
                var targetIndex = maxCountInPage * index + i;
                album.Set(allData[targetIndex]);
            }
        }
    }
}
