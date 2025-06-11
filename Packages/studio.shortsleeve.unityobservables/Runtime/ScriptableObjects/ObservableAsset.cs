using UnityEngine;

namespace UnityObservables
{
    public abstract class ObservableAsset<TPayload> : ScriptableObject
    {
        #region Inspector
        [SerializeField]
        Observable<TPayload> Observable;
        #endregion

        #region Public Properties
        public string DeveloperNotes
        {
            get => Observable.DeveloperNotes;
            set => Observable.DeveloperNotes = value;
        }
        #endregion

        #region Public API
        public int SubscriptionCount => Observable.SubscriptionCount;

        public void ClearSubscriptions() => Observable.ClearSubscriptions();

        public void Raise(TPayload payload) => Observable.Raise(payload);

        public void Subscribe(EventHandler<TPayload> handler) => Subscribe(handler, 0);

        public void Subscribe(EventHandler<TPayload> handler, int priority) =>
            Observable.Subscribe(handler, priority);

        public void Subscribe(IEventHandler<TPayload> handler) => Subscribe(handler, 0);

        public void Subscribe(IEventHandler<TPayload> handler, int priority) =>
            Observable.Subscribe(handler, priority);

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

        public void Unsubscribe(EventHandler<TPayload> handler) => Observable.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler<TPayload> handler) => Observable.Unsubscribe(handler);

        public TPayload Value
        {
            get => Observable.Value;
            set => Observable.Value = value;
        }
        #endregion
    }
}
