namespace Kalendra.Commons.Runtime.Architecture.Gateways
{
    public interface IRepository<T> : IReadOnlyRepository<T>, IWriteRepository<T>, IDeletionRepository { }
    public interface IBunchRepository<T> : IReadBunchRepository<T>, IRepository<T> { }
}