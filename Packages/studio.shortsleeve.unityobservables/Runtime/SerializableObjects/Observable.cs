using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityObservables
{
    [Serializable]
    public class Observable<TPayload> : EventBase<TPayload>
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
            return EqualityComparer<TPayload>.Default.Equals(first, second);
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
                Event.Raise(runtimeValue);
            }
        }

        public void Raise() => Event.Raise(Value);

        public void Subscribe(EventHandler<TPayload> handler) => Subscribe(handler, 0);

        public void Subscribe(EventHandler<TPayload> handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Subscribe(IEventHandler<TPayload> handler) => Subscribe(handler, 0);

        public void Subscribe(IEventHandler<TPayload> handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Unsubscribe(EventHandler<TPayload> handler) => Event.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler<TPayload> handler) => Event.Unsubscribe(handler);

        public void SubscribeAndTrigger(EventHandler<TPayload> handler)
        {
            Subscribe(handler, 0);
            handler.Invoke(Value);
        }

        public void SubscribeAndTrigger(EventHandler<TPayload> handler, int priority)
        {
            Subscribe(handler, priority);
            handler.Invoke(Value);
        }

        public void SubscribeAndTrigger(IEventHandler<TPayload> handler)
        {
            Subscribe(handler, 0);
            handler.HandleEvent(Value);
        }

        public void SubscribeAndTrigger(IEventHandler<TPayload> handler, int priority)
        {
            Subscribe(handler, priority);
            handler.HandleEvent(Value);
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
