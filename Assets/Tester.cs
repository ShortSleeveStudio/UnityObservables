using Studio.ShortSleeve.UnityObservables;
using UnityEngine;

public class Tester : MonoBehaviour
{
   [SerializeField] private ObservableInt observableInt;

    private float _timeSinceLastRaise;

    void OnEnable()
    {
        observableInt.Subscribe(OnEventFired);
    }

    void OnDisable()
    {
        observableInt.Unsubscribe(OnEventFired);
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

    void OnEventFired(int value)
    {
        Debug.Log("Value: " + value);
    }
}