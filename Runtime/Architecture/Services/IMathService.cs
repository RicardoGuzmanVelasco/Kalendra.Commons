namespace Kalendra.Commons.Runtime.Architecture.Services
{
    public interface IMathService : IMathClampService, IMathTupleService { }
    
    public interface IMathClampService
    {
        public int Clamp(int value, int min, int max);
        public int InverseClamp(int value, int min, int max);
        public int MirrorClamp(int value, int min, int max);
    }

    public interface IMathTupleService
    {
        public (int x, int y) Add((int x, int y) t1, (int x, int y) t2);
        
        /// <summary>
        /// Vectors having the same length but in the opposite direction are called negative vectors.
        /// </summary>
        public bool AreNegativeVectors((int x, int y) t1, (int x, int y) t2);
    }
}