namespace Studio.ShortSleeve.UnityObservables
{
    internal abstract class EventHandlerWrapper<TPayload>
    {
        public abstract object ID { get; }
        public abstract void HandleEvent(TPayload payload);
    }

    internal class EventHandlerWrapperDelegateVoid<TPayload> : EventHandlerWrapper<TPayload>
    {
        private readonly EventHandler _handler;
        public EventHandlerWrapperDelegateVoid(EventHandler handler) => _handler = handler;
        public override object ID => _handler;
        public override void HandleEvent(TPayload payload) => _handler.Invoke();
    }

    internal class EventHandlerWrapperDelegateGeneric<TPayload> : EventHandlerWrapper<TPayload>
    {
        private readonly EventHandler<TPayload> _handler;
        public EventHandlerWrapperDelegateGeneric(EventHandler<TPayload> handler) => _handler = handler;
        public override object ID => _handler;
        public override void HandleEvent(TPayload payload) => _handler.Invoke(payload);
    }

    internal class EventHandlerWrapperInterfaceVoid<TPayload> : EventHandlerWrapper<TPayload>
    {
        private readonly IEventHandler _handler;
        public EventHandlerWrapperInterfaceVoid(IEventHandler handler) => _handler = handler;
        public override object ID => _handler;
        public override void HandleEvent(TPayload payload) => _handler.HandleEvent();
    }

    internal class
        EventHandlerWrapperInterfaceGeneric<TPayload> : EventHandlerWrapper<TPayload>
    {
        private readonly IEventHandler<TPayload> _handler;
        public EventHandlerWrapperInterfaceGeneric(IEventHandler<TPayload> handler) => _handler = handler;
        public override object ID => _handler;
        public override void HandleEvent(TPayload payload) => _handler.HandleEvent(payload);
    }
}