using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paparazzi
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        public AudioSource BgSound;
        public List<AudioClip> BgList;


        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1)
        {
            if (arg0.name == "MainMenu")
            {
                BgSoundPlay(BgList[0]);
            }
            else
            {
                int cur_level = (int)System.Char.GetNumericValue(arg0.name[arg0.name.Length - 1]);
                for (int i = 1; i <= BgList.Count; i++)
                {
                    if (i == cur_level)
                        BgSoundPlay(BgList[cur_level]);
                }
            }
            
        }
        public void SFXPlay(string sfxName, AudioClip clip)
        {
            GameObject sound = new GameObject(sfxName + "Sound");
            AudioSource audiosource = sound.AddComponent<AudioSource>();
            audiosource.clip = clip;
            audiosource.Play();

            Destroy(sound, clip.length);
        }

        public void BgSoundPlay(AudioClip clip)
        {
            BgSound.clip = clip;
            BgSound.loop = true;
            BgSound.volume = 0.1f;
            BgSound.Play();
        }
    }
}
