using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    public abstract class EventAssetBase<TPayload> : ScriptableObject
    {
        #region Inspector

        [SerializeField]
        private string developerNotes;

#if UNITY_EDITOR
        [SerializeField]
        private TPayload raisePayload; // custom editor, don't change name
#endif

        #endregion

        #region Protected Properties

        protected readonly Event<TPayload> Event = new(); // custom editor, don't change name
        #endregion

        #region Public API

        public int SubscriptionCount => Event.SubscriptionCount;

        public void ClearSubscriptions() => Event.ClearSubscriptions();

        #endregion

        #region Private API

#if UNITY_EDITOR
        // custom editor, don't change name
        protected virtual void RaiseFromEditor() => Event.Raise(raisePayload);
#endif

        #endregion
    }
}
