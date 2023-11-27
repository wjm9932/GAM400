using System.Collections.Generic;
using UnityEngine;

namespace Paparazzi
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 3f;
        public float jumpVelocity = 20f;
        public float turnSmoothTime = 0.1f;
        public float currentSpeed => new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        private Camera followCamera;
        private PlayerInput input;
        private Animator animator;
        private CharacterController characterController;

        private float turnSmoothVelocity;
        private float currnetVelocityY = 0;
        private float lastGroundedTime;

        //public AudioClip Move_Clip;
        public AudioClip Jump_Clip;
        public AudioClip Land_Clip;

        void Start()
        {
            followCamera = Camera.main;
            input = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            if (input.moveInput.magnitude > 0.1f)
            {
                Rotate();
            }

            Move(input.moveInput);
        }

        void Update()
        {
            UpdateAnimation(input.moveInputForAnim);

            if (input.moveInput.magnitude > 0)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
            
            if (currnetVelocityY < -1f && IsGrounded() == false)
            {
                animator.SetBool("IsGrounded", false);
                animator.SetBool("IsFalling", true);
            }
            else
            {
                animator.SetBool("IsFalling", false);
            }

            if (Time.time - lastGroundedTime >= 0.1)
            {
                if (IsGrounded() == true)
                {
                    SoundManager.instance.SFXPlay("Land", Land_Clip);
                    animator.SetBool("IsGrounded", true);
                    animator.SetBool("IsJumping", false);
                }
            }

            if (input.isJump)
            {
                Jump();
            }

           

            if (IsGrounded() == true)
            {
                lastGroundedTime = Time.time;
            }
        }

        public void Move(Vector2 moveInput)
        {
            float targetSpeed = speed * moveInput.magnitude;
            Vector3 moveDir = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);

            currnetVelocityY += Time.deltaTime * Physics.gravity.y;
             
            Vector3 velocity = moveDir * targetSpeed;
           
            velocity = AdjustVelocityToSlope(velocity);
            velocity.y += currnetVelocityY;

            characterController.Move(velocity * Time.deltaTime);

            if (characterController.isGrounded == true)
            {
                currnetVelocityY = 0f;
            }
        }

        private Vector3 AdjustVelocityToSlope(Vector3 velocity)
        {
            var ray = new Ray(transform.position, Vector3.down);

            if(Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
            {
                var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                var adjustedVelocity = slopeRotation * velocity;

                if(adjustedVelocity.y < 0)
                {
                    return adjustedVelocity;
                }
            }

            return velocity;
        }

        public void Rotate()
        {
            var targetRotation = followCamera.transform.eulerAngles.y;
            targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetRotation;
        }

        public void Jump()
        {
            if (IsGrounded() == true)
            {
                SoundManager.instance.SFXPlay("Jump", Jump_Clip);
                animator.SetBool("IsGrounded", false);
                animator.SetBool("IsJumping", true);
                currnetVelocityY = jumpVelocity;
            }
            else
            {
                return;
            }
        }

        private void UpdateAnimation(Vector2 moveInput)
        {
            animator.SetFloat("Vertical", moveInput.y, 0.05f, Time.deltaTime);
            animator.SetFloat("Horizontal", moveInput.x, 0.05f, Time.deltaTime);
        }

        private bool IsGrounded()
        {
            if (characterController.isGrounded == true)
            {
                return true;
            }

            var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
            var maxDistance = 0.5f;
            Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
            return Physics.Raycast(ray, maxDistance);

        }

        //void Footstep()
        //{
        //    AudioSource.PlayClipAtPoint(Move_Clip, Camera.main.transform.position);
        //}
    }
}