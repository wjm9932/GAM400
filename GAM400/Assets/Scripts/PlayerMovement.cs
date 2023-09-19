using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float jumpVelocity = 20f;
    public float currentSpeed => new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

    private Camera followCamera;
    private PlayerInput input;
    private CharacterController characterController;

    private float currnetVelocityY;

    void Start()
    {
        followCamera = Camera.main;
        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if(currentSpeed > 2.0f)
        {
            Rotate();
        }

        Move(input.moveInput);
    }
   
    void Update()
    {
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
}
