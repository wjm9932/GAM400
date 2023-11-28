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

        public void MasterVolume(float val)
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(val) * 20);
        }
        public void BGSoundVolume(float val)
        {
            mixer.SetFloat("BGSoundVolume", Mathf.Log10(val) * 20);
        }
        public void SFXVolume(float val)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(val) * 20);
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
