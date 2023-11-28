using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 moveInput { get; private set; }
        public Vector2 moveInputForAnim { get; private set; }
        public bool isJump { get; private set; }

        // Update is called once per frame
        void Update()
        {
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            moveInputForAnim = moveInput;

            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput = moveInput.normalized;
            }
            isJump = Input.GetButtonDown("Jump");
        }
    }
}
