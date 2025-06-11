using System;
using UnityObservables;

public enum TestEnum
{
    Value1,
    Value2,
    Value3,
}

[Serializable]
public sealed class ObservableEnum : Observable<TestEnum> { }
