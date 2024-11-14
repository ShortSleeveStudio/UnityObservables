using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CreateAssetMenu(fileName = "EventVoid", menuName = "Unity Observables/Create EventVoid")]
    public class EventAssetVoid : EventAssetBase<bool>
    {
        public void Raise() => Event.Raise(default);

        public void Subscribe(EventHandler handler) => Event.Subscribe(handler);

        public void Subscribe(IEventHandler handler) => Event.Subscribe(handler);

        public void Unsubscribe(EventHandler handler) => Event.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler handler) => Event.Unsubscribe(handler);
    }
}
