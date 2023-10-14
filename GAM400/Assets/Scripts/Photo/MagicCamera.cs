using EzySlice;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Plane = UnityEngine.Plane;

namespace Paparazzi
{
    public class MagicCamera : MonoBehaviour
    {
        public RenderTexture polaroidTexture;
        public GameObject cameraViewer;
        public Camera mainCam;
        public Camera magicCam;
        public GameObject holder;
        public GameObject polaroidObject;
        public RawImage copyImage;
        public GameObject enviromentHolder;
        public float fovOffset = 5;

        private bool ActiveCapturedImage => hasPicture && !isCaptureMode;
        private bool isCaptureMode;
        private bool hasPicture;

        private Vector3[] currentNearCorners;
        private Vector3[] currentFarCorners;
        private Plane[] planes;

        void Start()
        {
            isCaptureMode = false;
            hasPicture = false;
            polaroidObject.SetActive(false);
            cameraViewer.SetActive(false);
        }

        void Update()
        {
            magicCam.transform.rotation = mainCam.transform.rotation;
            holder.transform.rotation = magicCam.transform.rotation;

            DrawFrustum();

            if (Input.GetKeyDown(KeyCode.F))
                isCaptureMode = !isCaptureMode;

            cameraViewer.SetActive(isCaptureMode);

            if (Input.GetMouseButtonDown(1) && isCaptureMode)
                Capture();

            if (ActiveCapturedImage && Input.GetMouseButtonDown(1))
                UseImage();

            polaroidObject.gameObject.SetActive(ActiveCapturedImage);
        }

        private void DrawFrustum()
        {
            magicCam.fieldOfView += fovOffset;

            planes = GeometryUtility.CalculateFrustumPlanes(magicCam);
            Vector3[] nearCorners = new Vector3[4];
            Vector3[] farCorners = new Vector3[4];

            (planes[1], planes[2]) = (planes[2], planes[1]);

            for (int i = 0; i < 4; ++i)
            {
                nearCorners[i] = Utility.Plane3Intersect(planes[4], planes[i], planes[(i + 1) % 4]);
                farCorners[i] = Utility.Plane3Intersect(planes[5], planes[i], planes[(i + 1) % 4]);
            }

            currentNearCorners = nearCorners;
            currentFarCorners = farCorners;

            magicCam.fieldOfView -= fovOffset;
        }

        private void Capture()
        {
            SetCapturedImageTexture();

            var viewingObjects = Utility.GetViewingObject(planes);

            foreach (var obj in viewingObjects)
            {
                GameObject newObject;
                GameObject slicedObject = Slice(obj, true);

                if (slicedObject == null)
                {
                    newObject = Instantiate(obj, obj.transform.position, obj.transform.rotation, holder.transform);
                }
                else
                {
                    newObject = Instantiate(slicedObject, slicedObject.transform.position, slicedObject.transform.rotation, holder.transform);
                    Destroy(slicedObject);
                }
                newObject.SetActive(false);
            }

            hasPicture = true;
        }

        private void SetCapturedImageTexture()
        {
            Texture2D cache = new Texture2D(polaroidTexture.width, polaroidTexture.height);
            RenderTexture.active = polaroidTexture;

            cache.ReadPixels(new Rect(0, 0, polaroidTexture.width, polaroidTexture.height), 0, 0);
            cache.Apply();

            RenderTexture.active = null;

            copyImage.texture = cache;
        }

        private GameObject Slice(GameObject target, bool getUpper)
        {
            bool hasSliced = false;
            GameObject current = null;
            GameObject upper = null;
            GameObject lower = null;

            for (int i = 0; i < currentNearCorners.Length; ++i)
            {
                if (hasSliced)
                {
                    target = current;
                    Destroy(current);
                }

                Material sliceMat = target.GetComponent<Renderer>().material;
                Vector3 norm = i + 1 >= currentFarCorners.Length ?
                    (currentFarCorners[i] + currentFarCorners[0]) / 2.0f : (currentFarCorners[i] + currentFarCorners[i + 1]) / 2.0f;
                Quaternion direction = Quaternion.LookRotation(norm);

                if (i == 1 || i == 3)
                    direction *= Quaternion.Euler(0, 0, 90);
                if (i == 3 || i == 2)
                    direction *= Quaternion.Euler(0, 0, 180);

                SlicedHull hull = target.Slice(magicCam.transform.position, direction * Vector3.up);
                if (hull != null)
                {
                    if (getUpper)
                    {
                        upper = hull.CreateUpperHull(target, sliceMat);
                        upper.tag = "Sliceable";
                        upper.transform.parent = enviromentHolder.transform;
                        current = upper;
                        if (upper.GetComponent<MeshCollider>() == null)
                            upper.AddComponent<MeshCollider>();
                    }
                    else
                    {
                        lower = hull.CreateLowerHull(target, sliceMat);
                        lower.tag = "Sliceable";
                        lower.transform.parent = enviromentHolder.transform;
                        current = target;
                        if (lower.GetComponent<MeshCollider>() == null)
                            lower.AddComponent<MeshCollider>();
                    }
                    hasSliced = true;
                }
            }

            return current;
        }

        private void UseImage()
        {
            var viewingObjects = Utility.GetViewingObject(planes);

            // delete and slice current viewing objects
            foreach (var obj in viewingObjects)
            {
                if (obj == null) continue;

                var sliced = Slice(obj, false);
                Destroy(obj);
            }

            //TODO: add returning animation about polaroid objects
            if (holder.transform.childCount <= 0)
                return;

            for (int i = 0; i < holder.transform.childCount; ++i)
            {
                var child = holder.transform.GetChild(i);
                var newObject = Instantiate(child, child.transform.position, child.transform.rotation, enviromentHolder.transform);
                newObject.gameObject.SetActive(true);
                newObject.tag = "Sliceable";
                if (child.GetComponent<MeshCollider>() == null)
                    child.AddComponent<MeshCollider>();
                if (child.GetComponent<MeshRenderer>() == null)
                    child.AddComponent<MeshRenderer>();
            }

            Utility.DestroyChildren(holder.transform);

            hasPicture = false;
        }
    }
}
