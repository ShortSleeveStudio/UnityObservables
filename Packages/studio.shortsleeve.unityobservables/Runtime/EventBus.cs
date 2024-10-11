using System.Collections.Generic;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    /// <summary>
    /// The EventBus is an internal class that satisfies the same function as a C# event. The only differences are:
    /// - It wraps event handlers in an abstract EventHandlerWrapper class to allow delegates and interfaces to
    ///   subscribe. Another benefit of using wrappers around the event handlers is that we can associate extra metadata
    ///   with our event handlers if we need to in the future.
    /// - It maintains a list and a dictionary of EventHandlerWrappers. The dictionary allows for idempotent semantics
    ///   as well as enabling lookup of an EventHandlerWrapper without a reference to it.
    /// </summary>
    public abstract class EventBus<TPayload> : ScriptableObject
    {
        #region Inspector

        [TextArea(1, 3)] [SerializeField] private string developerNotes;

        #endregion

        private readonly List<EventHandlerWrapper<TPayload>> _eventHandlerList; // custom editor, don't change name
        private readonly Dictionary<object, EventHandlerWrapper<TPayload>> _eventHandlerMap;

        protected EventBus()
        {
            _eventHandlerMap = new();
            _eventHandlerList = new();
        }

        #region Public API

        public int SubscriptionCount => _eventHandlerList.Count;


        public void ClearSubscriptions()
        {
            _eventHandlerMap.Clear();
            _eventHandlerList.Clear();
        }

        #endregion

        #region Protected API

        protected void RaiseInternal(TPayload payload)
        {
            for (int i = SubscriptionCount - 1; i >= 0; i--)
            {
                _eventHandlerList[i].HandleEvent(payload);
            }
        }

        protected void SubscribeInternal(EventHandler handler)
        {
            if (IsSubscribed(handler)) return;
            Subscribe(new EventHandlerWrapper<TPayload>(handler));
        }

        protected void SubscribeInternal(EventHandler<TPayload> handler)
        {
            if (IsSubscribed(handler)) return;
            Subscribe(new EventHandlerWrapper<TPayload>(handler));
        }

        protected void SubscribeInternal(IEventHandler handler)
        {
            if (IsSubscribed(handler)) return;
            Subscribe(new EventHandlerWrapper<TPayload>(handler));
        }

        protected void SubscribeInternal(IEventHandler<TPayload> handler)
        {
            if (IsSubscribed(handler)) return;
            Subscribe(new EventHandlerWrapper<TPayload>(handler));
        }

        protected void UnsubscribeInternal(EventHandler handler) => Unsubscribe(handler);

        protected void UnsubscribeInternal(EventHandler<TPayload> handler) => Unsubscribe(handler);

        protected void UnsubscribeInternal(IEventHandler handler) => Unsubscribe(handler);

        protected void UnsubscribeInternal(IEventHandler<TPayload> handler) => Unsubscribe(handler);

        #endregion

        #region Private API

        private void Subscribe(EventHandlerWrapper<TPayload> handlerWrapper)
        {
            _eventHandlerMap[handlerWrapper.ID] = handlerWrapper;
            _eventHandlerList.Add(handlerWrapper);
        }

        private void Unsubscribe(object handler)
        {
            if (!IsSubscribed(handler)) return;
            EventHandlerWrapper<TPayload> wrapper = _eventHandlerMap[handler];
            _eventHandlerMap.Remove(handler);
            _eventHandlerList.Remove(wrapper);
        }

        private bool IsSubscribed(object handler) => _eventHandlerMap.ContainsKey(handler);

        #endregion
    }
}