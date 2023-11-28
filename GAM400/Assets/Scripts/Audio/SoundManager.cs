using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace Paparazzi
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        public AudioMixer mixer;
        public AudioSource BgSound;
        public List<AudioClip> BgList;

        public const string MASTER_KEY = "masterVolume";
        public const string MUSIC_KEY = "musicVolume";
        public const string SFX_KEY = "sfxVolume";

        public const string MIXER_MASTER = "MasterVolume";
        public const string MIXER_MUSIC = "BGSoundVolume";
        public const string MIXER_SFX = "SFXVolume";

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
            LoadVolume();
        }

        private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1)
        {
            if (arg0.name == "MainMenu" || arg0.name == "SampleScene")
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

        void LoadVolume()
        {
            float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 0.5f);
            float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 0.5f);
            float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 0.5f);

            mixer.SetFloat(MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        }
        public void MasterVolume(float val)
        {
            mixer.SetFloat(MIXER_MASTER, Mathf.Log10(val) * 20);
        }
        public void BGSoundVolume(float val)
        {
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(val) * 20);
        }
        public void SFXVolume(float val)
        {
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(val) * 20);
        }

        public void SFXPlay(string sfxName, AudioClip clip)
        {
            GameObject sound = new GameObject(sfxName + "Sound");
            AudioSource audiosource = sound.AddComponent<AudioSource>();
            audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            audiosource.clip = clip;
            audiosource.Play();

            Destroy(sound, clip.length);
        }

        public void BgSoundPlay(AudioClip clip)
        {
            BgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
            BgSound.clip = clip;
            BgSound.loop = true;
            BgSound.volume = 0.1f;
            BgSound.Play();
        }
    }
}
