using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Paparazzi
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject volumeMenu;
        public static bool isPaused;
        public Button ResumeButton;
        public Button VolumeButton;
        public Button RestartButton;
        public Button GoToMenuButton;
        public Button QuitButton;
        public Button BackButton;

        public AudioClip Button_Clip;
        public AudioClip Pause_Clip;
        public AudioClip Resume_Clip;

        void Start()
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            volumeMenu.SetActive(false);

            ResumeButton.onClick.AddListener(ResumeGame);
            VolumeButton.onClick.AddListener(VolumeSettings);
            RestartButton.onClick.AddListener(RestartGame);
            GoToMenuButton.onClick.AddListener(GoToMainMenu);
            BackButton.onClick.AddListener(Back);
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

        public void RestartGame()
        {
            SoundManager.instance.SFXPlay("Button", Button_Clip);
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void VolumeSettings()
        {
            pauseMenu.SetActive(false);
            volumeMenu.SetActive(true);
        }

        public void Back()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            volumeMenu.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;

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

