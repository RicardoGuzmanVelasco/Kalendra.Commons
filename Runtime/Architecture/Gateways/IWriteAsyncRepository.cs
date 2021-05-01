using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Kalendra.Commons.Runtime.Architecture.Gateways
{
    public interface IWriteAsyncRepository<in T>
    {
        Task Save([NotNull] string hashID, [NotNull] T targetToSave);
    }
}