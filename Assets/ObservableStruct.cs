using System;
using UnityObservables;

[Serializable]
public class TestStruct
{
    public int TestInt;
    public double TestDouble;
    public string TestString;
}

[Serializable]
public sealed class ObservableStruct : Observable<TestStruct> { }
