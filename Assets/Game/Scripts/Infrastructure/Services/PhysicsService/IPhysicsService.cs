using UnityEngine;

namespace SwipeElements.Infrastructure.Services.PhysicsService
{
    public interface IPhysicsService
    {
        bool TryRayCastHit(Ray ray, int layer, out RaycastHit hit);
        bool TryRayCastHit(Ray ray, out RaycastHit hit);
        bool TryRayCastHit(Ray ray, int layer);
    }
}