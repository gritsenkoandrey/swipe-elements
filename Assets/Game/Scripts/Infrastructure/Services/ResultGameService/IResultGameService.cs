using System;

namespace SwipeElements.Infrastructure.Services.ResultGameService
{
    public interface IResultGameService
    {
        event Action<bool> OnResultGame;
        void SendResultGame(bool isWin);
    }
}