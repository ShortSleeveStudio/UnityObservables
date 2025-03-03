using System;
using Studio.ShortSleeve.UnityObservables;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField]
    private EventAssetVoid eventAssetVoid;

    [SerializeField]
    private ObservableAssetInt observableAssetInt;

    [SerializeField]
    private EventVoid eventVoid;

    [SerializeField]
    private ObservableInt observableInt;

    [SerializeField]
    private SubObject[] testSubObject;

    private float _timeSinceLastRaise;

    void OnEnable()
    {
        observableAssetInt.Subscribe(OnObservableAssetFired);
        eventAssetVoid.Subscribe(OnEventAssetVoidFired);
        observableInt.Subscribe(OnObservableIntFired);
        eventVoid.Subscribe(OnEventVoidFired);
        eventVoid.Subscribe(OnEventVoidFiredDuplicate);
        for (int i = 0; i < testSubObject.Length; i++)
            testSubObject[i].testSub.Subscribe(OnSubObjectChange);
    }

    void OnDisable()
    {
        observableAssetInt.Unsubscribe(OnObservableAssetFired);
        eventAssetVoid.Unsubscribe(OnEventAssetVoidFired);
        observableInt.Unsubscribe(OnObservableIntFired);
        eventVoid.Unsubscribe(OnEventVoidFired);
        eventVoid.Unsubscribe(OnEventVoidFiredDuplicate);
        for (int i = 0; i < testSubObject.Length; i++)
            testSubObject[i].testSub.Unsubscribe(OnSubObjectChange);
    }

    void Update()
    {
        if (_timeSinceLastRaise > 1f)
        {
            observableAssetInt.Value++;
            observableInt.Value++;
            for (int i = 0; i < testSubObject.Length; i++)
                testSubObject[i].testSub.Value++;

            _timeSinceLastRaise = 0;
        }

        _timeSinceLastRaise += Time.deltaTime;
    }

    void OnSubObjectChange(int value)
    {
        Debug.Log($"Sub Object Value {value}");
    }

    void OnEventAssetVoidFired()
    {
        Debug.Log("EventAssetVoid fired");
    }

    void OnObservableAssetFired(int value)
    {
        Debug.Log("ObservableAsset: " + value);
    }

    void OnEventVoidFired()
    {
        Debug.Log("EventVoid fired");
    }

    void OnEventVoidFiredDuplicate()
    {
        Debug.Log("EventVoid Duplicate Subscription fired");
    }

    public void OnObservableIntFired(int value)
    {
        Debug.Log("Observable: " + value);
    }

    [Serializable]
    public class SubObject
    {
        public ObservableInt testSub;
    }
}
