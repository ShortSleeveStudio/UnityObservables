using System.Collections.Generic;

namespace Studio.ShortSleeve.UnityObservables
{
    /// <summary>
    /// The Event is an internal class that satisfies the same function as a C# event. The only differences are:
    /// - It wraps event handlers in an abstract EventHandlerWrapper class to allow delegates and interfaces to
    ///   subscribe. Another benefit of using wrappers around the event handlers is that we can associate extra metadata
    ///   with our event handlers if we need to in the future.
    /// - It maintains a list and a dictionary of EventHandlerWrappers. The dictionary allows for idempotent semantics
    ///   as well as enabling lookup of an EventHandlerWrapper without a reference to it.
    /// </summary>
    public class Event<TPayload>
    {
        private readonly List<EventHandlerWrapper<TPayload>> _eventHandlerList;
        private readonly Dictionary<object, EventHandlerWrapper<TPayload>> _eventHandlerMap;

        public Event()
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

        public void Raise(TPayload payload)
        {
            for (int i = SubscriptionCount - 1; i >= 0; i--)
            {
                _eventHandlerList[i].HandleEvent(payload);
            }
        }

        public void Subscribe(EventHandler handler, int priority)
        {
            if (IsSubscribed(handler))
                return;
            SubscribeInternal(new EventHandlerWrapper<TPayload>(handler, priority));
        }

        public void Subscribe(EventHandler<TPayload> handler, int priority)
        {
            if (IsSubscribed(handler))
                return;
            SubscribeInternal(new EventHandlerWrapper<TPayload>(handler, priority));
        }

        public void Subscribe(IEventHandler handler, int priority)
        {
            if (IsSubscribed(handler))
                return;
            SubscribeInternal(new EventHandlerWrapper<TPayload>(handler, priority));
        }

        public void Subscribe(IEventHandler<TPayload> handler, int priority)
        {
            if (IsSubscribed(handler))
                return;
            SubscribeInternal(new EventHandlerWrapper<TPayload>(handler, priority));
        }

        public void Unsubscribe(EventHandler handler) => UnsubscribeInternal(handler);

        public void Unsubscribe(EventHandler<TPayload> handler) => UnsubscribeInternal(handler);

        public void Unsubscribe(IEventHandler handler) => UnsubscribeInternal(handler);

        public void Unsubscribe(IEventHandler<TPayload> handler) => UnsubscribeInternal(handler);

        #endregion

        #region Private API

        private void SubscribeInternal(EventHandlerWrapper<TPayload> handlerWrapper)
        {
            _eventHandlerMap[handlerWrapper.ID] = handlerWrapper;
            AddToSubscriberList(handlerWrapper);
        }

        private void UnsubscribeInternal(object handler)
        {
            if (!IsSubscribed(handler))
                return;
            EventHandlerWrapper<TPayload> wrapper = _eventHandlerMap[handler];
            _eventHandlerMap.Remove(handler);
            RemoveFromSubscriberList(wrapper);
        }

        private bool IsSubscribed(object handler) => _eventHandlerMap.ContainsKey(handler);

        private void AddToSubscriberList(EventHandlerWrapper<TPayload> handlerWrapper)
        {
            int index = _eventHandlerList.BinarySearch(handlerWrapper);
            _eventHandlerList.Insert(index < 0 ? ~index : index, handlerWrapper);
        }

        private void RemoveFromSubscriberList(EventHandlerWrapper<TPayload> handlerWrapper)
        {
            int index = _eventHandlerList.BinarySearch(handlerWrapper);
            if (index >= 0)
                _eventHandlerList.RemoveAt(index);
        }

        #endregion
    }
}
