using Kalendra.Commons.Tests.TestDoubles;
using NSubstitute;

namespace Kalendra.Commons.Tests.TestDataBuilders.StaticShortcuts
{
    public static class Fake
    {
        public static IEventListenerMock MockListener() => Substitute.For<IEventListenerMock>();
        public static IEventListenerMock<T> MockListener<T>() => Substitute.For<IEventListenerMock<T>>();
    }
}