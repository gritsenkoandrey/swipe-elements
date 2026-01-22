using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace SwipeElements.Game.Views
{
    public sealed class ElementAnimator : MonoBehaviour
    {
        [field:SerializeField] public SpriteRenderer Renderer { get; private set; }
        [SerializeField] private Sprite[] _idleSprites;
        [SerializeField] private Sprite[] _destroySprites;

        private Tween _tween;
        private int _index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartIdleAnimation(float duration)
        {
            TryKillTween();
            
            float time = duration / _destroySprites.Length;

            _index = Random.Range(0, _idleSprites.Length);
            _tween = DOVirtual.DelayedCall(time, UpdateIdleAnimation)
                .SetLoops(-1)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartDestroyAnimation(float duration)
        {
            TryKillTween();
            
            float time = duration / _destroySprites.Length;

            _index = -1;
            _tween = DOVirtual.DelayedCall(time, UpdateDestroyAnimation)
                .SetLoops(_destroySprites.Length)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateIdleAnimation()
        {
            _index = (_index + 1) % _idleSprites.Length;
            Renderer.sprite = _idleSprites[_index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateDestroyAnimation()
        {
            _index++;
            
            if (_index < _destroySprites.Length)
            {
                Renderer.sprite = _destroySprites[_index];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryKillTween()
        {
            if (_tween is {active: true})
            {
                _tween.Kill();
            }
        }
    }
}