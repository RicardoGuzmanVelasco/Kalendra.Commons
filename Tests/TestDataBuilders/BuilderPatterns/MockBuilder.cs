using NSubstitute;

namespace Kalendra.Commons.Tests.TestDataBuilders.Builders
{
    public abstract class MockBuilder<T> : Builder<T> where T : class
    {
        protected readonly T mock = Substitute.For<T>();
        
        public override T Build() => mock;
    }
}