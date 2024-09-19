using System;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryEvolved
{
    public class TickManager : MonoBehaviour
    {
        public static TickManager Instance { get; private set; }

        [SerializeField] public float TickTimerMax = 0.5f;
        [SerializeField] private float tickTimer;
        [SerializeField] private int tick;

        private List<Action> _actions = new List<Action>();
        private List<int> _intervals = new List<int>();
        private List<int> _tickCounts = new List<int>();

        private void Awake()
        {
            // Ensure there is only one instance of TickManager
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Subscribe(Action action, int interval)
        {
            _actions.Add(action);
            _intervals.Add(interval);
            _tickCounts.Add(0);
        }

        public void Unsubscribe(Action action)
        {
            int index = _actions.IndexOf(action);
            if (index != -1)
            {
                _actions.RemoveAt(index);
                _intervals.RemoveAt(index);
                _tickCounts.RemoveAt(index);
            }
        }

        private void Update()
        {
            tickTimer += Time.deltaTime;

            if (tickTimer <= TickTimerMax) return;
            tickTimer -= TickTimerMax;
            tick++;

            for (int i = 0; i < _actions.Count; i++)
            {
                _tickCounts[i]++;

                if (_tickCounts[i] <= _intervals[i]) continue;
                
                _tickCounts[i] = 0; 
                _actions[i].Invoke();
            }
        }
    }
}
