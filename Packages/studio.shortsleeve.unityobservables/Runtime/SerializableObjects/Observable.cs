using System;
using UnityEngine;

namespace UnityObservables
{
    [Serializable]
    public class Observable<TPayload> : EventGeneric<TPayload>
    {
        // The editor value doesn't change at runtime
        // We'll hide this at runtime, show it during edit time
        // We'll also change the name to "Value"
        [SerializeField]
        protected TPayload editorValue;

        // The runtime value does change at runtime
        // We'll show this at runtime, hide it during edit time
        [SerializeField]
        protected TPayload runtimeValue;

        // Tracks whether the runtime value has been initialized
        [NonSerialized]
        private bool _isRuntimeValueInitialized;

        #region Public API

        public virtual bool AreValuesEqual(TPayload first, TPayload second)
        {
            if (first == null)
                return second == null;
            return first.Equals(second);
        }

        public TPayload Value
        {
            get
            {
                EnsureInitialized();
                return runtimeValue;
            }
            set
            {
                EnsureInitialized();
                if (AreValuesEqual(runtimeValue, value) || !Application.isPlaying)
                    return;
                runtimeValue = value;
                Raise(runtimeValue);
            }
        }

        #endregion

        #region Private API

        private void EnsureInitialized()
        {
            if (_isRuntimeValueInitialized)
                return;
            _isRuntimeValueInitialized = true;
            runtimeValue = editorValue;
        }

#if UNITY_EDITOR
        // custom editor, don't change name
        protected override void RaiseFromEditor() => Event.Raise(runtimeValue);
#endif

        #endregion
    }
}
