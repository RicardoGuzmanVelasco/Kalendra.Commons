using System;
using FluentAssertions;
using NUnit.Framework;

namespace Kalendra.Maths.Tests.Editor
{
    public class IntervalTests
    {
        [Test]
        public void NewInterval_InvertedLimits_ThrowsException()
        {
            Action act = () => Interval.From(0, -1);

            act.Should().ThrowExactly<NotSupportedException>();
        }

        [Test]
        public void Middle_LimitsAreSame_ReturnsSame()
        {
            var sut = Interval.From(0, 0);

            var result = sut.Middle;

            result.Should().Be(0);
        }

        [Test]
        public void Middle_ReturnsAveragePoint()
        {
            var sut = Interval.From(3, 8);

            var result = sut.Middle;

            result.Should().Be(5.5f);
        }

        [Test]
        public void Length_LimitsAreSame_ReturnsZero()
        {
            var sut = Interval.From(1, 1);

            var result = sut.Length;

            result.Should().Be(0);
        }

        [Test]
        public void Length_ReturnsDistanceFromMinToMax()
        {
            var sut = Interval.From(5, 10);

            var result = sut.Length;

            result.Should().Be(5);
        }

        [Test]
        public void Includes_ValueOutOfTheInterval_ReturnsFalse()
        {
            var sut = Interval.From(0, 0);

            var result = sut.Includes(float.Epsilon);

            result.Should().BeFalse();
        }
        
        [Test]
        public void Includes_ValueWithinTheInterval_ReturnsTrue()
        {
            var sut = Interval.From(0, 1);

            var result = sut.Includes(float.Epsilon);

            result.Should().BeTrue();
        }

        [Test]
        public void Includes_IntervalOutOfTheInterval_ReturnsFalse()
        {
            var sut = Interval.From(0, 1);

            var result = sut.Includes(Interval.From(float.MinValue, 0));

            result.Should().BeFalse();
        }
        
        [Test]
        public void Includes_IntervalWithinTheInterval_ReturnsTrue()
        {
            var sut = Interval.From(0, 1);

            var result = sut.Includes(Interval.From(0, 0));

            result.Should().BeTrue();
        }

        [Test]
        public void Shift_DisplacesInterval_PositiveUnits()
        {
            var sut = Interval.From(0, 1);
            
            var result = sut.Shift(3);

            result.Should().Be(Interval.From(3, 4));
        }
        
        [Test]
        public void Shift_DisplacesInterval_NegativeUnits()
        {
            var sut = Interval.From(0, 1);
            
            var result = sut.Shift(-3);

            result.Should().Be(Interval.From(-3, -2));
        }
    }
}