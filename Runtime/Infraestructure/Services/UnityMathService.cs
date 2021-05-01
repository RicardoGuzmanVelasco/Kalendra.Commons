using Kalendra.Commons.Runtime.Architecture.Services;
using UnityEngine;

namespace Kalendra.Commons.Runtime.Infraestructure.Services
{
    public sealed class UnityMathService : IMathService
    {
        #region IMathClampService implementation
        public int Clamp(int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public int InverseClamp(int value, int min, int max)
        {
            if(value < min)
                value = max;
            if(value > max)
                value = min;
            return value;
        }

        public int MirrorClamp(int value, int min, int max)
        {
            while(value < min)
                value += max - min + 1;
            while(value > max)
                value -= max - min + 1;
            return value;
        }
        #endregion
        
        #region IMathTupleService implementation
        public (int x, int y) Add((int x, int y) t1, (int x, int y) t2)
        {
            return (t1.x + t2.x, t1.y + t2.y);
        }

        public bool AreNegativeVectors((int x, int y) t1, (int x, int y) t2)
        {
            return Add(t1, t2) == (0, 0);
        }
        #endregion
    }
}