using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class ProjectileTrigger : MonoBehaviour
    {
        public GameObject[] turnOff;
        public bool activated = false;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("Projectile") && !activated)
            {
                foreach (GameObject obj in turnOff)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
