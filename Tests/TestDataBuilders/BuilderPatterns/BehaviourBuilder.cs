using UnityEngine;

namespace Kalendra.Commons.Tests.TestDataBuilders.Builders
{
    public abstract class BehaviourBuilder<T> : Builder<T> where T : Component
    {
        public override T Build()
        {
            var dummyObject = new GameObject($"{typeof(T).Name}Behaviour (dummy)");
            return dummyObject.AddComponent<T>();
        }
    }
}