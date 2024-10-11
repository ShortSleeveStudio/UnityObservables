namespace Studio.ShortSleeve.UnityObservables
{
    public delegate void EventHandler();

    public delegate void EventHandler<in T>(T payload);

    public interface IEventHandler
    {
        void HandleEvent();
    }

    public interface IEventHandler<in T>
    {
        void HandleEvent(T payload);
    }
}