using System;
using UnityEngine;
using UnityObservables;

public class Tester : MonoBehaviour
{
    [SerializeField]
    private EventAssetVoid eventAssetVoid;

    [SerializeField]
    private ObservableAssetInt observableAssetInt;

    [SerializeField]
    private EventInt eventInt;

    [SerializeField]
    private EventVoid eventVoid;

    [SerializeField]
    private ObservableInt observableInt;

    [SerializeField]
    private SubObject[] testSubObject;

    private float _timeSinceLastRaise;

    void OnEnable()
    {
        observableAssetInt.SubscribeAndTrigger(OnObservableAssetFired);
        eventAssetVoid.Subscribe(OnEventAssetVoidFired);
        observableInt.Subscribe(OnObservableIntFired);
        eventInt.Subscribe(OnEventIntFired);
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
        eventInt.Unsubscribe(OnEventIntFired);
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

    void OnEventIntFired(int i)
    {
        Debug.Log("Int Event " + i);
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
