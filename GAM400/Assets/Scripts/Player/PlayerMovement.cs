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
        private float currnetVelocityY;

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

            if (input.isJump)
            {
                Jump();
            }
        }

        public void Move(Vector2 moveInput)
        {
            float targetSpeed = speed * moveInput.magnitude;
            Vector3 moveDir = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);

            currnetVelocityY += Time.deltaTime * Physics.gravity.y;

            Vector3 velocity = moveDir * targetSpeed + Vector3.up * currnetVelocityY;

            characterController.Move(velocity * Time.deltaTime);

            if (characterController.isGrounded == true)
            {
                currnetVelocityY = 0f;
            }
        }

        public void Rotate()
        {
            var targetRotation = followCamera.transform.eulerAngles.y;
            targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetRotation;
        }

        public void Jump()
        {
            if (characterController.isGrounded == true)
            {
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
    }

}