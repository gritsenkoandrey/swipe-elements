using System;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.ExitApplicationService
{
    public sealed class ExitApplicationService : MonoBehaviour, IExitApplicationService
    {
        public event Action OnExitGame = delegate { };
        
        public void OnApplicationQuit()
        {
            OnExitGame.Invoke();
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                OnExitGame.Invoke();
            }
        }
    }
}