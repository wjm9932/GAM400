using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paparazzi
{
    public class ZoneActivate : MonoBehaviour
    {
        public string Scene;
        public GameObject Player;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                SceneManager.LoadScene(Scene);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Player)
            {
                SceneManager.LoadScene(Scene);
            }
        }
    }
}
