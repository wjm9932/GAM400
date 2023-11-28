using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paparazzi
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public bool PlayerDead;
        public bool PlayingReviveSound;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }

            PlayerDead = false;
            PlayingReviveSound = false;
        }

        void Update()
        {
            if (PlayerDead)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                PlayerDead = false;
                PlayingReviveSound = true;
            }
        }
    }
}
