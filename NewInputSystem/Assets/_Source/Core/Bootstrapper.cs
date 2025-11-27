using Source.Core.InputSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Source.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float playerLookRange;

        [SerializeField] private InputListener inputListener;
        private void Awake()
        {
            PlayerMovement playerMovement = new PlayerMovement(playerTransform, playerLookRange);
            CameraZoom cameraZoom = new CameraZoom();
            
            inputListener.Construct(playerMovement, cameraZoom);
        }
    }
}