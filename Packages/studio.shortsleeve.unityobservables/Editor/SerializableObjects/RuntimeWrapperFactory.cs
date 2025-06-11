// using System;
// using System.Collections.Concurrent;
// using System.Reflection;
// using System.Reflection.Emit;
// using UnityEngine;

// namespace UnityObservables
// {
//     public class RuntimeWrapperFactory : MonoBehaviour
//     {
//         #region Static State
//         static readonly AssemblyBuilder AsmBuilder = AssemblyBuilder.DefineDynamicAssembly(
//             new AssemblyName("RuntimeWrappers"),
//             AssemblyBuilderAccess.Run
//         );
//         static readonly ModuleBuilder ModBuilder = AsmBuilder.DefineDynamicModule("MainModule");
//         static readonly ConcurrentDictionary<Type, Type> GeneratedTypes = new();
//         #endregion

//         #region Static API
//         public static SerializedFieldWrapperBase CreateWrapper(Type fieldType, object value)
//         {
//             Type concreteType = GeneratedTypes.GetOrAdd(fieldType, GenerateWrapperType);
//             SerializedFieldWrapperBase instance =
//                 ScriptableObject.CreateInstance(concreteType) as SerializedFieldWrapperBase;
//             instance.hideFlags = HideFlags.DontSave;
//             instance.SetValue(value);
//             return instance;
//         }

//         private static Type GenerateWrapperType(Type fieldType)
//         {
//             string typeName = $"RuntimeWrapper_{fieldType.FullName.Replace('.', '_')}";
//             Type baseGeneric = typeof(SerializedFieldWrapper<>).MakeGenericType(fieldType);
//             TypeBuilder tb = ModBuilder.DefineType(
//                 typeName,
//                 TypeAttributes.Public | TypeAttributes.Class,
//                 baseGeneric
//             );
//             return tb.CreateTypeInfo();
//         }
//         #endregion
//     }
// }
