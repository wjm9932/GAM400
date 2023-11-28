using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Paparazzi
{
    public class Blur_value : MonoBehaviour
    {
        public Material BlurMat;
        public float BlurLevel = 0f;
        private bool Isincreasing = true;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            BlurMat.SetFloat("_BlurLevel", BlurLevel);
            if (Isincreasing)
            {
                BlurLevel += Time.deltaTime / 100f;
                if (BlurLevel > 0.01f)
                {
                    Isincreasing = false;
                }
            }
                if (!Isincreasing)
                {
                    BlurLevel -= Time.deltaTime / 100f;
                    if (BlurLevel < 0f)
                    {
                        Isincreasing = true;
                    }
                }
            }
        
    }
}
