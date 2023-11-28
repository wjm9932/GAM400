using UnityEngine;

namespace Paparazzi
{
    public class KillboxReset : MonoBehaviour
    {
        public GameObject Player;

        // Start is called before the first frame update
        void Start()
        {
            if (Player == null)
            {
                Player = GameObject.Find("Main Player");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Player)
            {
                LevelManager.Instance.PlayerDead = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == Player)
            {
                LevelManager.Instance.PlayerDead = true;
            }
        }
    }
}
