using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float _horizontalMove;
    float _verticalMove;

    [HideInInspector]
    public CharacterController _charController;
    public Ray _landingRay;
    public RaycastHit _hit;

    [SerializeField]
    public bool _canJump;
    public bool _inAir;
    float _fallVelocity;

    public float _playerSpeed;
    public float _gravity;
    public float _jumpForce;
    public float _hightLimit;

    void Start()
    {
        _canJump = false;
        _inAir = false;
        _charController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");
        _verticalMove = Input.GetAxis("Vertical");

        _landingRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * _hightLimit);

        SetGravity();
        PlayerJump();
    }

    private void FixedUpdate()
    {
        _charController.Move(new Vector3(_horizontalMove, _fallVelocity, _verticalMove) * _playerSpeed * Time.deltaTime);
    }

    private void PlayerJump()
    {
        if (Physics.Raycast(_landingRay, out _hit, _hightLimit) || _charController.isGrounded)
        {
            _canJump = true;
            _inAir = false;

            if (Input.GetButtonDown("Jump"))
            {
                _fallVelocity = _jumpForce;
            }
        }
        else
        {
            _canJump = false;
            _inAir = true;
        }
    }

    private void SetGravity()
    {
        if (_charController.isGrounded)
        {
            _fallVelocity = -_gravity * Time.deltaTime;
        }
        else
        {
            _fallVelocity -= _gravity * Time.deltaTime;
        }
    }
}
