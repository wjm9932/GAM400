using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class Utility
    {
        public static void DestroyChildren(Transform root)
        {
            foreach (Transform child in root.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        public static Vector3 Plane3Intersect(Plane p1, Plane p2, Plane p3)
        {
            return ((-p1.distance * Vector3.Cross(p2.normal, p3.normal)) +
                    (-p2.distance * Vector3.Cross(p3.normal, p1.normal)) +
                    (-p3.distance * Vector3.Cross(p1.normal, p2.normal))) /
                   (Vector3.Dot(p1.normal, Vector3.Cross(p2.normal, p3.normal)));
        }

        public static GameObject[] GetViewingObject(Plane[] planes)
        {
            List<GameObject> lists = new List<GameObject>();
            var slicables = GameObject.FindGameObjectsWithTag("Sliceable");
            foreach (var obj in slicables)
            {
                var renderer = obj.GetComponent<Renderer>();

                var result = GeometryUtility.TestPlanesAABB(planes, renderer.bounds);

                if (result)
                    lists.Add(obj);
            }

            return lists.ToArray();
        }
    }
}
