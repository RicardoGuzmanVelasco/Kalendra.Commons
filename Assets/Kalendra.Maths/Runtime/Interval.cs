using System;
using System.Collections;
using System.Collections.Generic;

namespace Kalendra.Maths
{
    public readonly struct Interval
    {
        readonly int min;
        readonly int max;

        Interval(int min, int max)
        {
            if(min > max)
                throw new NotSupportedException("Cannot create inverted interval");
            
            this.min = min;
            this.max = max;
        }
        
        #region Factory methods
        public static Interval From(int min, int max) => new Interval(min, max);
        public static Interval From(int minmax) => new Interval(minmax, minmax);

        public static Interval Naturals { get; } = From(1, int.MaxValue);
        public static Interval Integers { get; } = From(int.MinValue, int.MaxValue);
        public static Interval NonNegativeIntegers { get; } = From(0, int.MaxValue);
        #endregion

        public float Middle => (max + min) / 2f;
        public int Length => max - min;

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

        public Interval Shift(int offset)
        {
            return From(min + offset, max + offset);
        }

        public IEnumerable<int> IterateOver()
        {
            for(var i = min; i <= max; i++)
                yield return i;
        }
        
        #region Operators overloading
        public static bool operator <(Interval interval, float number)
        {
            return interval.max < number;
        }

        public static bool operator >(Interval interval, float number)
        {
            return interval.min > number;
        }
        #endregion
        
        #region Conversions
        public static implicit operator (float min, float max)(Interval source)
        {
            return (source.min, source.max);
        }

        public static implicit operator Interval((int min, int max) source)
        {
            return From(source.min, source.max);
        }
        #endregion
    }
}