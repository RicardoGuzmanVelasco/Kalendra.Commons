namespace Kalendra.Commons.Runtime.Architecture.Gateways
{
    public interface IAsyncRepository<T> : IReadOnlyAsyncRepository<T>, IWriteAsyncRepository<T>, IDeletionAsyncRepository { }
}