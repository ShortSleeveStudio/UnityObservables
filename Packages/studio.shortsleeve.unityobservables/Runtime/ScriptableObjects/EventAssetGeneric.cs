namespace Studio.ShortSleeve.UnityObservables
{
    public abstract class EventAssetGeneric<TPayload> : EventAssetBase<TPayload>
    {
        public void Raise(TPayload payload) => Event.Raise(payload);

        public void Subscribe(EventHandler<TPayload> handler) => Subscribe(handler, 0);

        public void Subscribe(EventHandler<TPayload> handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Subscribe(IEventHandler<TPayload> handler) => Subscribe(handler, 0);

        public void Subscribe(IEventHandler<TPayload> handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Unsubscribe(EventHandler<TPayload> handler) => Event.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler<TPayload> handler) => Event.Unsubscribe(handler);
    }
}
