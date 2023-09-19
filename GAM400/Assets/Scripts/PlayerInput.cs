using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 moveInput { get; private set; }
    public bool isJump { get; private set; }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput = moveInput.normalized;
        }
        isJump = Input.GetButtonDown("Jump");
    }
}
