using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _execQueue = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        GameObject obj = new("MainThreadDispatcher");
        obj.AddComponent<MainThreadDispatcher>();
        DontDestroyOnLoad(obj);
    }

    void Update()
    {
        while (_execQueue.Count > 0)
        {
            Action action;
            lock (_execQueue)
            {
                action = _execQueue.Dequeue();
            }

            action?.Invoke();
        }
    }

    public static void Enqueue(Action action)
    {
        lock (_execQueue)
        {
            _execQueue.Enqueue(action);
        }
    }
}
