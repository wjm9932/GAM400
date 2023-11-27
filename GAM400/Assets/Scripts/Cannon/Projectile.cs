using UnityEngine;

namespace Paparazzi
{
    public class Projectile : MonoBehaviour
    {
        public float life = 5f;
        private float timer = 0.0f;

        private void Start()
        {
            timer = 0.0f;
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= life)
                Destroy(gameObject);
        }
    }
}
