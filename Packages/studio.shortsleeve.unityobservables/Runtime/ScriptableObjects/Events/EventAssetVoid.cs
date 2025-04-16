using UnityEngine;

namespace UnityObservables
{
    [CreateAssetMenu(fileName = "EventVoid", menuName = "Unity Observables/Create EventVoid")]
    public class EventAssetVoid : ScriptableObject
    {
        #region Inspector
        [SerializeField]
        EventVoid Event = new();
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

        public void Raise() => Event.Raise();

        public void Subscribe(EventHandler handler) => Subscribe(handler, 0);

        public void Subscribe(EventHandler handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Subscribe(IEventHandler handler) => Subscribe(handler, 0);

        public void Subscribe(IEventHandler handler, int priority) =>
            Event.Subscribe(handler, priority);

        public void Unsubscribe(EventHandler handler) => Event.Unsubscribe(handler);

        public void Unsubscribe(IEventHandler handler) => Event.Unsubscribe(handler);
        #endregion
    }
}
