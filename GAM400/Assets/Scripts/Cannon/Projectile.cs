using UnityEngine;

namespace Paparazzi
{
    public class Projectile : MonoBehaviour
    {
        public float life = 5f;
        private float timer = 0.0f;
        private bool activeTimer = true;

        private void Start()
        {
            timer = 0.0f;

            activeTimer = gameObject.tag != "Sliceable_M";
        }

        void Update()
        {
            if (!activeTimer)
                return;

            timer += Time.deltaTime;

            if (timer >= life)
                Destroy(gameObject);
        }
    }
}
