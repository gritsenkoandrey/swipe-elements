using System.Collections;
using UnityEngine;

namespace SwipeElements.Infrastructure.Services.CoroutineService
{
    public interface ICoroutineService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
        void StopAllCoroutines();
    }
}