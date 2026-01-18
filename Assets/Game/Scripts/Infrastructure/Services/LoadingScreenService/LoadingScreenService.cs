using UnityEngine;

namespace SwipeElements.Infrastructure.Services.LoadingScreenService
{
    public sealed class LoadingScreenService : MonoBehaviour, ILoadingScreenService
    {
        [SerializeField] private Canvas _canvas;

        void ILoadingScreenService.Show()
        {
            _canvas.enabled = true;
        }

        void ILoadingScreenService.Hide()
        {
            _canvas.enabled = false;
        }
    }
}