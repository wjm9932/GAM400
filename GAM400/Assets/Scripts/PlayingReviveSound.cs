using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Paparazzi
{
    public class PlayingReviveSound : MonoBehaviour
    {
        public AudioClip Revive_Clip;

        void Start()
        {

        }

        void Update()
        {
            if (LevelManager.Instance.PlayingReviveSound)
            {
                StartCoroutine(PlaySound());
                LevelManager.Instance.PlayingReviveSound = false;
            }
        }

        IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(0.1f);
            SoundManager.instance.SFXPlay("Revive", Revive_Clip);
        }
    }
}
