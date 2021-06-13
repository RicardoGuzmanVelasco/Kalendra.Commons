using System.Threading.Tasks;

namespace Kalendra.Commons.Runtime.Architecture.Boundaries
{
    public interface IAsyncBoundaryInputPort
    {
        Task Request();
    }

    public interface IAsyncBoundaryInputPort<in TArg> where TArg : IBoundaryRequestDTO
    {
        Task Request(TArg requestDTO);
    }
}