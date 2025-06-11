using UnityEngine;

namespace UnityObservables
{
    [CreateAssetMenu(
        fileName = "Data",
        menuName = "ScriptableObjects/SpawnManagerScriptableObject"
    )]
    public class SerializedFieldWrapper : ScriptableObject
    {
        [SerializeReference]
        public object Value;
    }
}
