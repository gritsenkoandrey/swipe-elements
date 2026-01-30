using UnityEngine;

namespace SwipeElements.Infrastructure.Services.CameraService
{
    public interface ICameraService
    {
        Camera MainCamera { get; }
        void AdaptiveCamera(Bounds bounds);
        (float right, float left) CalculateHorizontalScreenBounds();
    }
}