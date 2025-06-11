using UnityEngine;

namespace UnityObservables
{
    public abstract class SerializedFieldWrapperBase : ScriptableObject
    {
        public abstract object GetValue();

        public abstract void SetValue(object value);
    }
}
