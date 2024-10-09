// using UnityEngine;
//
// namespace studio.shortsleeve.unityobservables
// {
//     /// <summary>
//     /// Base event object from which all other events and observables descend. 
//     /// </summary>
//     public abstract class EventBase<TPayload> : ScriptableObject, IEventBus<TPayload>
//     {
//         #region Inspector
//
//         [SerializeField] private string developerNotes;
//
//         #endregion
//
//         #region Constructor
//
//         private readonly EventBus<TPayload> _eventBus = new();
//
//         #endregion
//
//         #region Event Bus
//
//         public int SubscriptionCount => _eventBus.SubscriptionCount;
//         public void Subscribe(EventHandler handler) => _eventBus.Subscribe(handler);
//         public void Subscribe(EventHandler<TPayload> handler) => _eventBus.Subscribe(handler);
//         public void Subscribe(IEventHandler handler) => _eventBus.Subscribe(handler);
//         public void Subscribe(IEventHandler<TPayload> handler) => _eventBus.Subscribe(handler);
//         public void Unsubscribe(object handler) => _eventBus.Unsubscribe(handler);
//         public void Raise(TPayload payload) => _eventBus.Raise(payload);
//         public void ClearSubscriptions() => _eventBus.ClearSubscriptions();
//
//         #endregion
//     }
// }

