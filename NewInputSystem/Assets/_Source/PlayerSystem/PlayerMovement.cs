using Source.Core.InputSystem;
using UnityEngine;

public class PlayerMovement
{
    private float _upDownLookRange;
    private Camera _camera;
    private Transform _playerTransform;
    private CharacterController _charController;

    private float _verticalRotation;

    private Vector3 _currMovement;

    private InputListener _inputListener;

    public PlayerMovement(Transform playerTransform, float upDownLookRange)
    {
        _upDownLookRange = upDownLookRange;
        _playerTransform = playerTransform;
        _camera = Camera.main;
        _charController = _playerTransform.GetComponent<CharacterController>();
    }

    public void Move(Vector2 movementV2, float movementSpeed, InputListener inputListener, float jumpForce)
    {
        _inputListener = inputListener;

        Vector3 worldDirection = CalculateWorldDirection(movementV2);
        _currMovement.x = worldDirection.x * movementSpeed;
        _currMovement.z = worldDirection.z * movementSpeed;

        HandleJumping(inputListener, jumpForce);
        _charController.Move(_currMovement * Time.deltaTime);
    }

    public void HandleRotation(Vector2 rotationAmountV2, float mouseSensitivity)
    {
        float mouseXRotation = rotationAmountV2.x * mouseSensitivity;
        float mouseYRotation = rotationAmountV2.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }

    private void ApplyHorizontalRotation(float rotationAmount)
    {
        _playerTransform.Rotate(0, rotationAmount, 0);
    }
    
    private void ApplyVerticalRotation(float rotationAmount)
    {
        _verticalRotation = Mathf.Clamp(_verticalRotation - rotationAmount, -_upDownLookRange, _upDownLookRange);
        _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    private void HandleJumping(InputListener inputListener, float jumpForce)
    {
        if (_charController.isGrounded)
        {
            _currMovement.y = -0.5f;
            if (inputListener.JumpTriggered)
            {
                _currMovement.y = jumpForce;
            }
        }
        else
        {
            _currMovement.y = Physics.gravity.y * Time.deltaTime;
        }
    }

    private Vector3 CalculateWorldDirection(Vector2 movementV2)
    {
        Vector3 inputDirection = new Vector3(movementV2.x, 0, movementV2.y);
        Vector3 worldDirection = _playerTransform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }
}
