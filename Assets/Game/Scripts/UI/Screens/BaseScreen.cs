using UnityEngine;

namespace SwipeElements.UI.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

        public void Show()
        {
            _canvas.enabled = true;
            
            Subscribe();
        }
        
        public void Hide()
        {
            _canvas.enabled = false;
            
            Unsubscribe();
        }
    }
}