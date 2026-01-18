using UnityEngine;

namespace SwipeElements.Infrastructure.Services.CameraService
{
    public sealed class CameraService : MonoBehaviour, ICameraService
    { 
        [SerializeField] private Camera _mainCamera;

        Camera ICameraService.MainCamera => _mainCamera;
    }
}