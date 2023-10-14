using UnityEngine;

namespace Paparazzi
{
    public class LevelManager : MonoBehaviour
    {
        public int CurrentLevelMaxCount;

        [SerializeField] private MagicPhotoPlayer player;

        void Start()
        {
            player.SetCaptureMaxCount(CurrentLevelMaxCount);
        }

        void Update()
        {

        }
    }
}
