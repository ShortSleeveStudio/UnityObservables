namespace Studio.ShortSleeve.UnityObservables
{
    public delegate void EventHandler();

    public delegate void EventHandler<in T>(T payload);

    public interface IEventHandler
    {
        public void HandleEvent();
    }

    public interface IEventHandler<in T>
    {
        public void HandleEvent(T payload);
    }
}