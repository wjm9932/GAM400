using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Paparazzi.MovableObject;

namespace Paparazzi
{
    public class MovableObject : MonoBehaviour
    {
        public enum MovingType
        {
            LEFT, RIGHT, UP, DOWN,
            TOP_LEFT, TOP_RIGHT,
            BOTTOM_LEFT, BOTTOM_RIGHT
        }

        public float moveDistance = 5f;
        public float moveSpeed = 2f;
        public MovingType movingtype = MovingType.LEFT;
        private Vector3 originalPosition;
        private Vector3 movementDirection;

        void Start()
        {
            originalPosition = transform.position;
            SetMovementDirection();
        }

        void Update()
        {
            transform.Translate(movementDirection * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(originalPosition, transform.position) >= moveDistance)
            {
                originalPosition = transform.position;
                ReverseDirection();
                SetMovementDirection();
            }
        }

        void SetMovementDirection()
        {
            switch (movingtype)
            {
                case MovingType.LEFT:
                    movementDirection = Vector3.left;
                    break;
                case MovingType.RIGHT:
                    movementDirection = Vector3.right;
                    break;
                case MovingType.UP:
                    movementDirection = Vector3.forward;
                    break;
                case MovingType.DOWN:
                    movementDirection = Vector3.back;
                    break;
                case MovingType.TOP_LEFT:
                    movementDirection = (Vector3.left + Vector3.forward).normalized;
                    break;
                case MovingType.TOP_RIGHT:
                    movementDirection = (Vector3.right + Vector3.forward).normalized;
                    break;
                case MovingType.BOTTOM_LEFT:
                    movementDirection = (Vector3.left + Vector3.back).normalized;
                    break;
                case MovingType.BOTTOM_RIGHT:
                    movementDirection = (Vector3.right + Vector3.back).normalized;
                    break;
            }

        }

        void ReverseDirection()
        {
            switch (movingtype)
            {
                case MovingType.LEFT:
                    movingtype = MovingType.RIGHT;
                    break;
                case MovingType.RIGHT:
                    movingtype = MovingType.LEFT;
                    break;
                case MovingType.UP:
                    movingtype = MovingType.UP;
                    break;
                case MovingType.DOWN:
                    movingtype = MovingType.DOWN;
                    break;
                case MovingType.TOP_LEFT:
                    movingtype = MovingType.BOTTOM_RIGHT; 
                    break;
                case MovingType.TOP_RIGHT:
                    movingtype = MovingType.BOTTOM_LEFT;
                    break;
                case MovingType.BOTTOM_LEFT:
                    movingtype = MovingType.TOP_RIGHT;
                    break;
                case MovingType.BOTTOM_RIGHT:
                    movingtype = MovingType.TOP_LEFT;
                    break;
            }
        }
    }
}
