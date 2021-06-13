namespace Kalendra.Commons.Tests.TestDoubles
{
    public interface IEventListenerMock
    {
        void Call();
    }
    
    public interface IEventListenerMock<in T>
    {
        void Call(T param);
    }
}