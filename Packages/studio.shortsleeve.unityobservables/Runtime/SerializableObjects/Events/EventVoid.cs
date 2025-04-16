using System;

namespace UnityObservables
{
    [Serializable]
    public class EventVoid : EventBase<bool>
    {
        public void Raise() => Event.Raise(default);

        public void Subscribe(EventHandler handler) => Subscribe(handler, 0);

        public void Subscribe(EventHandler handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Subscribe(IEventHandler handler) => Subscribe(handler, 0);

        public void Subscribe(IEventHandler handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Unsubscribe(EventHandler handler) => Event.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler handler) => Event.Unsubscribe(handler);
    }
}
