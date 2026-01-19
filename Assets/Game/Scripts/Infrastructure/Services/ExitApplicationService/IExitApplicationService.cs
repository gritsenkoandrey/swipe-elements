using System;

namespace SwipeElements.Infrastructure.Services.ExitApplicationService
{
    public interface IExitApplicationService
    {
        event Action OnExitGame;
    }
}