using System;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    public abstract class Observable<TPayload> : EventGeneric<TPayload>
    {
        // The editor value doesn't change at runtime
        // We'll hide this at runtime, show it during edit time
        // We'll also change the name to "Value"
        [SerializeField] protected TPayload _editorValue;

        // The runtime value does change at runtime
        // We'll show this at runtime, hide it during edit time
        [SerializeField] protected TPayload _runtimeValue;

        // Tracks whether the runtime value has been initialized
        [NonSerialized] private bool _isRuntimeValueInitialized;


        #region Public API

        public virtual bool AreValuesEqual(TPayload first, TPayload second)
        {
            if (first == null)
                return second != null;
            return first.Equals(second);
        }

        public TPayload Value
        {
            get
            {
                EnsureInitialized();
                return _runtimeValue;
            }
            set
            {
                EnsureInitialized();
                if (AreValuesEqual(_runtimeValue, value))
                    return;
                _runtimeValue = value;
                if (Application.isPlaying)
                    Raise(_runtimeValue);
            }
        }

        #endregion

        #region Private API

        private void EnsureInitialized()
        {
            if (_isRuntimeValueInitialized)
                return;
            _isRuntimeValueInitialized = true;
            _runtimeValue = _editorValue;
        }

        #endregion
    }
}