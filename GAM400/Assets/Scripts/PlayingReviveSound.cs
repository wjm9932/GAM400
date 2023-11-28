using UnityEngine;

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
                SoundManager.instance.SFXPlay("Revive", Revive_Clip);
                LevelManager.Instance.PlayingReviveSound = false;
            }
        }
    }
}
