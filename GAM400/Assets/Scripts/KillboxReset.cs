using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == Player)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
