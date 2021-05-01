using JetBrains.Annotations;

namespace Kalendra.Commons.Runtime.Architecture.Gateways
{
    public interface IWriteRepository<in T>
    {
        void Save([NotNull] T targetToSave, [CanBeNull] string hashID = "");
    }
}