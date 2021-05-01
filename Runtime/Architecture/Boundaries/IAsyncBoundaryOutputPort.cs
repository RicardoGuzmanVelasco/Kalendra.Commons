using System;
using System.Threading.Tasks;

namespace Kalendra.Commons.Runtime.Architecture.Boundaries
{
    public interface IBoundaryOutputPort
    {
        void Response();
    }

    public interface IBoundaryOutputPort<in T> where T : IBoundaryResponseDTO
    {
        void Response(T responseDTO);
    }
    
    public interface IAsyncBoundaryOutputPort
    {
        Task Response();
    }

    public interface IAsyncBoundaryOutputPort<in T> where T : IBoundaryResponseDTO
    {
        Task Response(T responseDTO);
    }

    public interface IBoundaryResponseDTO { }
}