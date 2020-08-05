using System;
using UniRx;
using UnityEngine;

namespace SA.SpaceShooter
{
    public class MonoActionTimer : MonoBehaviour
    {
        #region Var

        CompositeDisposable compositeDisposable;

        #endregion


        #region Timer

        protected void ActionTimer(float time, Action act)
        {
            compositeDisposable = new CompositeDisposable();

            Observable.Timer(TimeSpan.FromSeconds(time))
            .Subscribe(_ =>
            {
                act?.Invoke();
            })
            .AddTo(compositeDisposable);
        }


        protected void OnDispose()
        {
            compositeDisposable?.Dispose();
        }

        #endregion
    }
}