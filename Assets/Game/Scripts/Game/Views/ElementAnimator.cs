using System.Runtime.CompilerServices;
using LitMotion;
using UnityEngine;

namespace SwipeElements.Game.Views
{
    public sealed class ElementAnimator : MonoBehaviour
    {
        [field:SerializeField] public SpriteRenderer Renderer { get; private set; }
        
        [SerializeField] private Sprite[] _idleSprites;
        [SerializeField] private Sprite[] _destroySprites;

        private MotionHandle _handle;
        private int _startFrame;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartIdleAnimation(float duration)
        {
            TryCancelHandle();
            
            _startFrame = Random.Range(0, _idleSprites.Length);
            _handle = LMotion.Create(0f, _idleSprites.Length, duration)
                .WithEase(Ease.Linear)
                .WithLoops(-1)
                .Bind(UpdateIdleAnimation)
                .AddTo(this, LinkBehavior.CancelOnDisable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartDestroyAnimation(float duration)
        {
            TryCancelHandle();
            
            _handle = LMotion.Create(0f, _destroySprites.Length, duration)
                .WithEase(Ease.Linear)
                .Bind(UpdateDestroyAnimation)
                .AddTo(this, LinkBehavior.CancelOnDisable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateIdleAnimation(float value)
        {
            int frame = (int)(value + _startFrame) % _idleSprites.Length;
            
            Renderer.sprite = _idleSprites[frame];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateDestroyAnimation(float value)
        {
            int frame = (int)value;
                    
            if (frame >= _destroySprites.Length)
            {
                frame = _destroySprites.Length - 1;
            }
                    
            Renderer.sprite = _destroySprites[frame];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryCancelHandle()
        {
            if (_handle.IsActive())
            {
                _handle.Cancel();
            }
        }
    }
}