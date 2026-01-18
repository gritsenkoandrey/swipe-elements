using JetBrains.Annotations;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.ApplicationService
{
    [UsedImplicitly]
    public sealed class ApplicationService : IApplicationService
    {
        void IApplicationService.Init()
        {
            Application.targetFrameRate = 60;
        }
    }
}