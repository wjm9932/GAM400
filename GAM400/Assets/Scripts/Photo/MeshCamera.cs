using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Paparazzi
{
    public class MeshCamera : MonoBehaviour
    {
        public Camera cam;
        public MagicPhoto photo;

        public Transform captureRoot, spawnRoot;

        private MeshCutter meshCutter;
        private Plane plane;

        public void Capture()
        {
            captureRoot.DestroyChildren();
            photo.GetPhotoScreenBounds(out var min, out var max);
            var rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);

            //TODO: Get Object only including min and max size, Currently get objects all Sliceable and check Min Max
            var sliceAble = GameObject.FindGameObjectsWithTag("Sliceable");
            foreach (var obj in sliceAble)
            {
                var mr = obj.GetComponent<MeshRenderer>();
                var mf = obj.GetComponent<MeshFilter>();

                if (mr == null || mf == null || !obj.activeSelf)
                    continue;

                var r = GetViewPointRect(mr.bounds);
                if (rect.Overlaps(r, true))
                    Slice(mf);
            }
        }

        public void Release()
        {
            photo.GetPhotoScreenBounds(out var min, out var max);
            var rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
            var sliceable = GameObject.FindGameObjectsWithTag("Sliceable");

            foreach (var obj in sliceable)
            {
                var mr = obj.GetComponent<MeshRenderer>();
                var mf = obj.GetComponent<MeshFilter>();

                if (mr == null || mf == null || !obj.activeSelf)
                    continue;

                var r = GetViewPointRect(mr.bounds);
                if (rect.Overlaps(r, true))
                    RemoveSlice(mf);
            }

            while (captureRoot.childCount > 0)
            {
                var t = captureRoot.GetChild(0);
                t.parent = spawnRoot;
                t.gameObject.SetActive(true);
            }
        }

        void Start()
        {
            meshCutter = new MeshCutter(256);
            plane = new Plane();
        }

        private Rect GetViewPointRect(Bounds bounds)
        {
            var center = bounds.center;
            var extents = bounds.extents;

            var extentPoints = new Vector2[]
            {
                cam.WorldToViewportPoint(new Vector3(center.x - extents.x, center.y, center.z - extents.z)),
                cam.WorldToViewportPoint(new Vector3(center.x + extents.x, center.y, center.z - extents.z)),
                cam.WorldToViewportPoint(new Vector3(center.x - extents.x, center.y, center.z + extents.z)),
                cam.WorldToViewportPoint(new Vector3(center.x + extents.x, center.y, center.z + extents.z))
            };

            var min = extentPoints[0];
            var max = extentPoints[0];

            foreach (var v in extentPoints)
            {
                min = Vector2.Min(min, v);
                max = Vector2.Max(max, v);
            }

            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }

        private void Slice(MeshFilter mf)
        {
            var original = mf.sharedMesh;
            var mesh = new Mesh
            {
                vertices = original.vertices,
                triangles = original.triangles,
                normals = original.normals,
                uv = original.uv,
            };

            for (int i = 0; i < 4; ++i)
            {
                GetCutPlaneOfSurface(mf.transform, i);
                if (!meshCutter.SliceMesh(mesh, ref plane))
                {
                    if (plane.GetDistanceToPoint(meshCutter.GetFirstVertex()) > 0)
                        return;
                }
                else
                {
                    MeshFromTempMesh(mesh, meshCutter.NegativeMesh);
                }
            }

            var sliced = GameObjectFromMesh(mf.gameObject, mesh);
            sliced.SetActive(false);
        }

        private void GetCutPlaneOfSurface(Transform t, int i)
        {
            photo.GetPhotoScreenBounds(out var min, out var max);

            Vector3 p0, p1;
            if (i == 0)
            {
                p0 = min;
                p1 = new Vector3(min.x, max.y);
            }
            else if (i == 1)
            {
                p0 = new Vector3(min.x, max.y);
                p1 = max;
            }
            else if (i == 2)
            {
                p0 = max;
                p1 = new Vector3(max.x, min.y);
            }
            else
            {
                p0 = new Vector3(max.x, min.y);
                p1 = min;
            }

            var startRay = cam.ViewportPointToRay(p0);
            var endRay = cam.ViewportPointToRay(p1);

            var start = startRay.GetPoint(cam.nearClipPlane);
            var end = endRay.GetPoint(cam.nearClipPlane);
            var depth = endRay.direction.normalized;

            var tangent = (end - start).normalized;
            if (tangent == Vector3.zero)
                tangent = Vector3.right;

            var normal = Vector3.Cross(depth, tangent);

            var localPosition = t.InverseTransformPoint(start);
            var transformedNormal = ((Vector3)(t.localToWorldMatrix.transpose * normal)).normalized;
            plane.SetNormalAndPosition(transformedNormal, localPosition);
        }

        private void MeshFromTempMesh(Mesh mesh, TempMesh temp)
        {
            mesh.Clear();
            mesh.SetVertices(temp.vertices);
            mesh.SetTriangles(temp.triangles, 0);
            mesh.SetNormals(temp.normals);
            mesh.SetUVs(0, temp.uvs);
            mesh.RecalculateTangents();
        }

        private Mesh MeshFromTempMesh(TempMesh temp)
        {
            var mesh = new Mesh();
            MeshFromTempMesh(mesh, temp);
            return mesh;
        }

        private GameObject GameObjectFromMesh(GameObject obj, Mesh mesh)
        {
            var instance = Instantiate(obj, captureRoot, true);
            instance.GetComponent<MeshFilter>().sharedMesh = mesh;
            var prevCollider = instance.GetComponent<Collider>();
            if (!(prevCollider is MeshCollider))
                Destroy(prevCollider);

            var newCollider = instance.AddComponent<MeshCollider>();
            newCollider.sharedMesh = mesh;
            newCollider.convex = true;
            return instance;
        }

        private void RemoveSlice(MeshFilter mf)
        {
            var original = mf.sharedMesh;
            var mesh = new Mesh
            {
                vertices = original.vertices,
                triangles = original.triangles,
                normals = original.normals,
                uv = original.uv,
            };

            for (int i = 0; i < 4; ++i)
            {
                GetCutPlaneOfSurface(mf.transform, i);
                if (!meshCutter.SliceMesh(mesh, ref plane))
                {
                    if (plane.GetDistanceToPoint(meshCutter.GetFirstVertex()) > 0)
                        return;
                }
                else
                {
                    MeshFromTempMesh(mesh, meshCutter.NegativeMesh);
                    GameObjectFromMesh(mf.gameObject, MeshFromTempMesh(meshCutter.PositiveMesh));
                }
            }

            Destroy(mf.gameObject);
        }

    }
}
