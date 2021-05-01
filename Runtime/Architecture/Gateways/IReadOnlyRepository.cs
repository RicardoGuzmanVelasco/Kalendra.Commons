using System.Collections.Generic;
using JetBrains.Annotations;

namespace Kalendra.Commons.Runtime.Architecture.Gateways
{
    public interface IReadBunchRepository<out T>
    {
        IEnumerable<T> LoadAll();
    }

    public interface IReadOnlyRepository<out T>
    {
        T Load([NotNull] string hashID);
    }

    public interface IReadRepository<out T> : IReadOnlyRepository<T>, IReadBunchRepository<T> { }
}