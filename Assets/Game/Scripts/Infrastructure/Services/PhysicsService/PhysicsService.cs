using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.PhysicsService
{
    [UsedImplicitly]
    public sealed class PhysicsService : IPhysicsService
    {
        private const float DISTANCE = float.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IPhysicsService.TryRayCastHit(Ray ray, int layer, out RaycastHit hit)
        {
            if (Physics.Raycast(ray, out hit, DISTANCE, layer))
            {
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRayCastHit(Ray ray, out RaycastHit hit)
        {
            if (Physics.Raycast(ray, out hit, DISTANCE))
            {
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IPhysicsService.TryRayCastHit(Ray ray, int layer)
        {
            if (Physics.Raycast(ray, DISTANCE, layer))
            {
                return true;
            }

            return false;
        }
    }
}