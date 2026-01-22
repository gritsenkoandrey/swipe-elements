using System;
using JetBrains.Annotations;

namespace SwipeElements.Infrastructure.Services.ResultGameService
{
    [UsedImplicitly]
    public sealed class ResultGameService : IResultGameService
    {
        public event Action<bool> OnResultGame = delegate { };
        
        void IResultGameService.SendResultGame(bool isWin) => OnResultGame.Invoke(isWin);
    }
}