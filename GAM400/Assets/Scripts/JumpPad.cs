using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class JumpPad : MonoBehaviour
    {
        public CharacterController characterController;

        // Update is called once per frame
        private void Start()
        {
            characterController = GetComponent<CharacterController>();

        }
        void Update()
        {
            //characterController
            //characterController.Move(Vector3.up * Time.deltaTime);

        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
                characterController.Move(Vector3.up * 500f);//Time.deltaTime);
        }
    }
}
