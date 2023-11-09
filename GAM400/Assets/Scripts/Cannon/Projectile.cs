using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class Projectile : MonoBehaviour
    {
        public float life = 5f;

        private void Awake()
        {
            Destroy(gameObject, life);
        }
    }
}
