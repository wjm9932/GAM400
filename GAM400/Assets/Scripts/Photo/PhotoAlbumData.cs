using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    [Serializable]
    public class PhotoData
    {
        public Texture2D Image;
        public GameObject Holder;

        public PhotoData(Texture2D image, GameObject holder)
        {
            Image = image;
            Holder = holder;
        }
    }

    public class PhotoAlbumData
    {
        public List<PhotoData> Albumes { get; private set; }

        public void Add(PhotoData data)
        {
            Albumes.Add(data);
        }

        public void Remove(GameObject targetHolder)
        {
            var target = Albumes.Find(data => data.Holder == targetHolder);
            Albumes.Remove(target);
        }

        public PhotoAlbumData()
        {
            Albumes = new List<PhotoData>();
        }
    }
}
