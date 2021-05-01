namespace Kalendra.Commons.Runtime.Architecture.Boundaries
{
    public interface IBoundaryInputPort
    {
        void Request();
    }
    
    public interface IBoundaryInputPort<in TArg> where TArg : IBoundaryRequestDTO
    {
        void Request(TArg requestDTO);
    }
}