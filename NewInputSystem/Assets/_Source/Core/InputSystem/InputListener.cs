using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Core.InputSystem
{
    public class InputListener : MonoBehaviour
    {
        private MainInputSystem _mainInputSystem;

        private InputAction _movement;
        private InputAction _lookRotation;
        private InputAction _jump;
        private InputAction _zoom;
        private InputAction _fromPlayerToUIMap;
        private InputAction _switchBetweenTwoActionMaps;

        private PlayerMovement _playerMovement;
        private CameraZoom _cameraZoom;

        [SerializeField] private float _playerSpeed;
        [SerializeField] private float _playerSensitivity;

        [SerializeField] private float _jumpForce;

        [SerializeField] private float _zoomSpeed;

        [SerializeField] private CharacterController _charController; 

        public bool JumpTriggered { get; private set; }

        private bool _isActionMapPlayer = true;
 
        public void Construct(PlayerMovement playerMovement, CameraZoom cameraZoom)
        {
            _playerMovement = playerMovement;
            _cameraZoom = cameraZoom;
        }

        private void Start()
        {
            _mainInputSystem = new();

            Bind();
        }

        private void Bind()
        {
            _movement = _mainInputSystem.Player.Move;
            _movement.Enable();

            _lookRotation = _mainInputSystem.Player.Look;
            _lookRotation.Enable();

            _jump = _mainInputSystem.Player.Jump;
            _jump.Enable();

            _zoom = _mainInputSystem.Player.Zoom;
            _zoom.Enable();

            _jump.performed += input_info => JumpTriggered = true;
            _jump.canceled += input_info => JumpTriggered = false;

            _switchBetweenTwoActionMaps = _mainInputSystem.UI.Interact;
            _switchBetweenTwoActionMaps.Enable();
            _switchBetweenTwoActionMaps.performed += SwitchBetweenActionMaps;
        }
        
        private void SwitchBetweenActionMaps(InputAction.CallbackContext context)
        {
            if (_isActionMapPlayer)
            {
                _isActionMapPlayer = !_isActionMapPlayer;
                _mainInputSystem.Player.Disable();
                _mainInputSystem.UI.Enable();
            }
            else
            {
                _isActionMapPlayer = !_isActionMapPlayer;
                _mainInputSystem.UI.Disable();
                _mainInputSystem.Player.Enable();

                _switchBetweenTwoActionMaps.Enable();
            }
            
        }

        private void FixedUpdate()
        {
            Movement();
            LookRotation();
            Zoom();
        }

        private void Movement()
        {
            Vector2 movementV2 = _movement.ReadValue<Vector2>();
            _playerMovement.Move(movementV2, _playerSpeed, this, _jumpForce);
        }

        private void LookRotation()
        {
            Vector2 lookRotationV2 = _lookRotation.ReadValue<Vector2>();
            _playerMovement.HandleRotation(lookRotationV2, _playerSensitivity);
        }

        private void Zoom()
        {
            float zoomF = _zoom.ReadValue<float>();
            _cameraZoom.Zoom(zoomF, _zoomSpeed);
        }

        private void OnDisable() => Unbind();

        private void Unbind()
        {
            _movement.Disable();
            _lookRotation.Disable();
            _jump.Disable();
            _zoom.Disable();

            _mainInputSystem.Player.Disable();
            _mainInputSystem.UI.Disable();
        }
    }
}