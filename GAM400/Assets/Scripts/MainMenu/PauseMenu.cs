using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Paparazzi
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public static bool isPaused;
        public Button ResumeButton;
        public Button GoToMenuButton;
        public Button QuitButton;

        public AudioClip Button_Clip;
        public AudioClip Pause_Clip;
        public AudioClip Resume_Clip;

        void Start()
        {
            isPaused = false;
            pauseMenu.SetActive(false);

            ResumeButton.onClick.AddListener(ResumeGame);
            GoToMenuButton.onClick.AddListener(GoToMainMenu);
            QuitButton.onClick.AddListener(Quit);
        }


        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void PauseGame()
        {
            SoundManager.instance.SFXPlay("Pause", Pause_Clip);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void ResumeGame()
        {
            SoundManager.instance.SFXPlay("Resume", Resume_Clip);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void GoToMainMenu()
        {
            SoundManager.instance.SFXPlay("Button", Button_Clip);
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        public void Quit()
        {
            SoundManager.instance.SFXPlay("Button", Button_Clip);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

