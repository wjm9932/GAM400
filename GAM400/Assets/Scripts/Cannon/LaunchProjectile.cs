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

        public AudioSource Fire_Sound;

        private void Update()
        {
            Sec += Time.deltaTime;
            if (Sec > 1)
            {
                RandNum = Random.Range(0, projectile.Count);

                var _projectile = Instantiate(projectile[RandNum], launchPoint.position, launchPoint.rotation);
 
                Fire_Sound.outputAudioMixerGroup = SoundManager.instance.mixer.FindMatchingGroups("SFX")[0];
                Fire_Sound.Play();

                _projectile.GetComponent<Rigidbody>().velocity = -launchPoint.right * launchVelocity;
                Sec = 0;
            }

        }
    }
}
