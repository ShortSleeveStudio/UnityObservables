namespace Studio.ShortSleeve.UnityObservables
{
    public abstract class EventGeneric<TPayload> : EventBus<TPayload>
    {
        public void Raise(TPayload payload) => RaiseInternal(payload);
        public void Subscribe(EventHandler<TPayload> handler) => SubscribeInternal(handler);
        public void Subscribe(IEventHandler<TPayload> handler) => SubscribeInternal(handler);
        public void Unsubscribe(EventHandler<TPayload> handler) => UnsubscribeInternal(handler);
        public void Unsubscribe(IEventHandler<TPayload> handler) => UnsubscribeInternal(handler);
    }
}