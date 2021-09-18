using System;

namespace Kalendra.Maths
{
    public readonly struct Interval
    {
        readonly float min;
        readonly float max;

        Interval(float min, float max)
        {
            if(min > max)
                throw new NotSupportedException("Cannot create inverted interval");
            
            this.min = min;
            this.max = max;
        }
        
        #region Factory methods
        public static Interval From(float min, float max) => new Interval(min, max);
        #endregion

        public float Middle => (max + min) / 2;
        public float Length => max - min;

        #region Includes
        public bool Includes(float value)
        {
            return min <= value && value <= max;
        }

        public bool Includes(Interval other)
        {
            return Includes(other.min) && Includes(other.max);
        }
        #endregion

        public Interval Shift(float offset)
        {
            return From(min + offset, max + offset);
        }
        
        #region Conversions
        public static implicit operator (float min, float max)(Interval source)
        {
            return (source.min, source.max);
        }

        public static implicit operator Interval((float min, float max) source)
        {
            return From(source.min, source.max);
        }
        #endregion
    }
}