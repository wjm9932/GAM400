using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class LaunchProjectile : MonoBehaviour
    {
        public Transform launchPoint;
        public List<GameObject> projectile = new List<GameObject>();
        public float launchVelocity = 15f;
        private int RandNum = 0;
        private float Sec = 0;
        
        private void Update()
        {
            Sec += Time.deltaTime;
            if(Sec > 1)
            {
                RandNum = Random.Range(-1, 2);
                var _projectile = Instantiate(projectile[RandNum], launchPoint.position, launchPoint.rotation);
                _projectile.GetComponent<Rigidbody>().velocity = -launchPoint.right * launchVelocity;
                Sec = 0;
            }

        }
    }
}
