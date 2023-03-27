using System;

namespace OneBunny
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FSMStateAttribute : Attribute
    {
        public readonly int key;

        public FSMStateAttribute(int key)
        {
            this.key = key;
        }
    }
    public abstract class FSMState<T> where T : IFSMRunner
    {
        protected readonly T runnerEntity;

        public virtual void BeginState() {}

        public virtual void UpdateState(){}

        public virtual void FixedUpdateState(){}

        public virtual void ExitState(){}

        public FSMState(IFSMRunner runner)
        {
            runnerEntity = (T)runner;
        }
    }
}