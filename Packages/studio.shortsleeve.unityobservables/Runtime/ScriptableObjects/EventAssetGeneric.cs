namespace Studio.ShortSleeve.UnityObservables
{
    public abstract class EventAssetGeneric<TPayload> : EventAssetBase<TPayload>
    {
        public void Raise(TPayload payload) => Event.Raise(payload);

        public void Subscribe(EventHandler<TPayload> handler) => Event.Subscribe(handler);

        public void Subscribe(IEventHandler<TPayload> handler) => Event.Subscribe(handler);

        public void Unsubscribe(EventHandler<TPayload> handler) => Event.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler<TPayload> handler) => Event.Unsubscribe(handler);
    }
}
