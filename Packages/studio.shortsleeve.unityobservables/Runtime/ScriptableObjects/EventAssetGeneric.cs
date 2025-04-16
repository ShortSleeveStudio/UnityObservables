using UnityEngine;

namespace UnityObservables
{
    public abstract class EventAssetGeneric<TEvent, TType> : ScriptableObject
        where TEvent : EventGeneric<TType>, new()
    {
        #region Inspector
        [SerializeField]
        TEvent Event = new();
        #endregion

        #region Public Properties
        public string DeveloperNotes
        {
            get => Event.DeveloperNotes;
            set => Event.DeveloperNotes = value;
        }
        #endregion

        #region Public API
        public int SubscriptionCount => Event.SubscriptionCount;

        public void ClearSubscriptions() => Event.ClearSubscriptions();

        public void Raise(TType payload) => Event.Raise(payload);

        public void Subscribe(EventHandler<TType> handler) => Subscribe(handler, 0);

        public void Subscribe(EventHandler<TType> handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Subscribe(IEventHandler<TType> handler) => Subscribe(handler, 0);

        public void Subscribe(IEventHandler<TType> handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Unsubscribe(EventHandler<TType> handler) => Event.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler<TType> handler) => Event.Unsubscribe(handler);
        #endregion
    }
}
