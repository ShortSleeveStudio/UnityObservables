using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CreateAssetMenu(fileName = "EventVoid", menuName = "Unity Observables/Create EventVoid")]
    public class EventVoid : EventBus<bool>
    {
        public void Subscribe(EventHandler handler) => SubscribeInternal(handler);
        public void Subscribe(IEventHandler handler) => SubscribeInternal(handler);
        public void Unsubscribe(EventHandler handler) => UnsubscribeInternal(handler);
        public void Unsubscribe(IEventHandler handler) => UnsubscribeInternal(handler);
    }
}