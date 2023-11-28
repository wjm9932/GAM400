using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paparazzi
{
    public class MainMenu : MonoBehaviour
    {
        public AudioClip Button_Clip;
        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnClickStart()
        {
            SoundManager.instance.SFXPlay("Button", Button_Clip);
            SceneManager.LoadScene("Level1");
        }
        public void OnClickOption()
        {
            SoundManager.instance.SFXPlay("Button", Button_Clip);
            SceneManager.LoadScene("Test_PhotoFeature");
        }

        public void OnClickTestLevel()
        {
            SoundManager.instance.SFXPlay("Button", Button_Clip);
            SceneManager.LoadScene("SampleScene");
        }
        public void OnClickEnd()
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
