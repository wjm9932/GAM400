using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public static class Utility
    {
        public static void DestroyChildren(this Transform root)
        {
            foreach (Transform child in root.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }
    }
}
