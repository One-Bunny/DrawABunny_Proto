using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OneBunny
{
    public interface IFSMRunner
    {
    }

    public abstract class FSMRunner<T> : MonoBehaviour where T : IFSMRunner
    {
        private Dictionary<int, FSMState<T>> _states = new Dictionary<int, FSMState<T>>();

        protected T runner;

        private FSMState<T> _currentState = null;
        private FSMState<T> _previousState = null;

        protected virtual void Update()
        {
            _currentState?.UpdateState();
        }

        protected virtual void FixedUpdate()
        {
            _currentState?.FixedUpdateState();
        }
        
        protected void SetUp(ValueType firstState)
        {
            _states = new Dictionary<int, FSMState<T>>();
            var stateTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(FSMState<T>) != t && typeof(FSMState<T>).IsAssignableFrom(t));

            foreach (var stateType in stateTypes)
            {
                var attribute = stateType.GetCustomAttribute<FSMStateAttribute>();

                if (attribute == null)
                {
                    continue;
                }

                var state = Activator.CreateInstance(stateType, this as IFSMRunner) as FSMState<T>;

                if (!_states.TryAdd(attribute.key, state))
                {
                    Debug.LogError($"[FSM ERROR] {typeof(T)} 의 {attribute.key} 키가 중복되었습니다.");
                }
            }

            ChangeState(firstState);
        }
        
        public void ChangeState(ValueType enumValue)
        {
            if (!_states.TryGetValue((int)enumValue, out var state))
            {
                Debug.LogError($"[FSM ERROR] {GetType()} : 사용 불가 상태. {enumValue}");
                return;
            }

            ChangeState(state);
        }

        public void ChangeState(FSMState<T> newState)
        {
            if (newState == null || newState == _currentState)
            {
                return;
            }

            if (_currentState != null)
            {
                _previousState = _currentState;
                _currentState.ExitState();
            }
            
            _currentState = newState;
            _currentState.BeginState();
            
            Debug.Log($"[FSM CHANGED] {_currentState}으로 변경");
        }

        public void GetPrevious()
        {
            if (_previousState == null)
            {
                return;
            }

            ChangeState(_previousState);
        }
    }
}