using UnityEngine;

namespace SwipeElements.Infrastructure.Services.CameraService
{
    public sealed class CameraService : MonoBehaviour, ICameraService
    { 
        [SerializeField] private Camera _mainCamera;

        Camera ICameraService.MainCamera => _mainCamera;

        void ICameraService.AdaptiveCamera(Bounds bounds)
        {
            float screenRatio = (float)Screen.width / Screen.height;
            float targetRatio = bounds.size.x / bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                float differenceInSize = targetRatio / screenRatio;
                
                _mainCamera.orthographicSize = bounds.size.y / 2f * differenceInSize;
            }
            else
            {
                _mainCamera.orthographicSize = bounds.size.y / 2f;
            }

            float bottomY = bounds.min.y;
            float topY = bounds.max.y;
            float cameraBottomPosition = bottomY + _mainCamera.orthographicSize;
            float cameraTopPosition = topY - _mainCamera.orthographicSize;
            float targetY = Mathf.Lerp(cameraBottomPosition, cameraTopPosition, 0f);
            Vector3 newPosition = _mainCamera.transform.position;
            newPosition.y = targetY;
            newPosition.x = bounds.center.x;
            
            _mainCamera.transform.position = newPosition;
        }
    }
}