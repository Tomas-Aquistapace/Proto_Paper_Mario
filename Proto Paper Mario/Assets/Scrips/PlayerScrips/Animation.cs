using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator _animator;
    public PlayerMovement _playerMovement;
    public float _rotationSpeed;

    bool left;
    bool right;
    bool lastDirection = true;

    static float _ROTATION = 180;

    float leftNumber = _ROTATION;
    float rightNumber = _ROTATION;

    float horizontalMove;
    float verticalMove;

    void Update()
    {
        RotationSprite();
        RunAnimation();
        JumpAnimation();
    }

    void RunAnimation()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            _animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }
        else if (Input.GetKey("w") || Input.GetKey("s"))
        {
            _animator.SetFloat("Speed", Mathf.Abs(verticalMove));
        }else
        {
            _animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }
    }

    void JumpAnimation()
    {
        if (_playerMovement._canJump && !_playerMovement._inAir)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _animator.SetBool("IsJumping", true);
            }
            _animator.SetBool("IsJumping", false);
        }
        else
        {
            _animator.SetBool("IsJumping", true);
        }
    }

    void RotationSprite()
    {
        if (Input.GetKeyDown("a") || left == true)
        {
            if (lastDirection == true)
            {
                if (leftNumber > 0)
                {
                    transform.Rotate(new Vector3(0f, _rotationSpeed, 0f) * Time.deltaTime);
                    left = true;

                    leftNumber -= _rotationSpeed * Time.deltaTime;
                }
                else
                {
                    left = false;
                    leftNumber = _ROTATION;

                    transform.eulerAngles = new Vector3(0, 180, 0);

                    lastDirection = false;
                }
            }
        }
        else if (Input.GetKeyDown("d") || right == true)
        {
            if (lastDirection == false)
            {
                if (rightNumber > 0)
                {
                    transform.Rotate(new Vector3(0f, -_rotationSpeed, 0f) * Time.deltaTime);
                    right = true;

                    rightNumber -= _rotationSpeed * Time.deltaTime;
                }
                else
                {
                    right = false;
                    rightNumber = _ROTATION;

                    transform.eulerAngles = new Vector3(0, 0, 0);

                    lastDirection = true;
                }
            }
        }
    }
}
