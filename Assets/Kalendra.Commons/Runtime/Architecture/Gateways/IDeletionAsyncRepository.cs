using System.Threading.Tasks;

namespace Kalendra.Commons.Runtime.Architecture.Gateways
{
    public interface IDeletionAsyncRepository
    {
        Task<bool> Delete(string hashID);
    }
}