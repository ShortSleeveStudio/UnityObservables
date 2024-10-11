using Studio.ShortSleeve.UnityObservables;
using UnityEngine;

public class Tester : MonoBehaviour, IEventHandler<int>
{
    [SerializeField] private ObservableInt observableInt;

    private float _timeSinceLastRaise;

    void OnEnable()
    {
        observableInt.Subscribe(OnEventFired0);
        observableInt.Subscribe(OnEventFired1);
        observableInt.Subscribe(HandleEvent);
    }

    void OnDisable()
    {
        observableInt.Unsubscribe(OnEventFired0);
        observableInt.Unsubscribe(OnEventFired1);
        observableInt.Unsubscribe(HandleEvent);
    }

    void Update()
    {
        if (_timeSinceLastRaise > 1f)
        {
            observableInt.Value++;
            _timeSinceLastRaise = 0;
        }

        _timeSinceLastRaise += Time.deltaTime;
    }

    void OnEventFired0(int value)
    {
        Debug.Log("Value 0: " + value);
    }

    void OnEventFired1(int value)
    {
        Debug.Log("Value 1: " + value);
    }

    public void HandleEvent(int value)
    {
        Debug.Log("Value 2: " + value);
    }
}