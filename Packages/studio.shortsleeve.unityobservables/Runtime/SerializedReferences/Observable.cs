using System;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [Serializable]
    public class Observable<TPayload> : EventGeneric<TPayload>
    {
        [SerializeField] protected TPayload value;

        #region Public API

        public virtual bool AreValuesEqual(TPayload first, TPayload second)
        {
            if (first == null)
                return second != null;
            return first.Equals(second);
        }

        public TPayload Value
        {
            get => value;
            set
            {
                if (AreValuesEqual(this.value, value) || !Application.isPlaying)
                    return;
                this.value = value;
                Raise(this.value);
            }
        }

        #endregion

        #region Private API

#if UNITY_EDITOR
        // custom editor, don't change name
        protected override void RaiseFromEditor() => Event.Raise(value);
#endif

        #endregion
    }
}