using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Kalendra.Maths.Tests.Editor
{
    public class IntervalTests
    {
        #region Fixture
        public static int AnyPositiveNumber => 100;
        #endregion
        
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
        public void Middle_AllIntegers_IsZero()
        {
            var sut = Interval.Integers;

            var result = sut.Middle;

            result.Should().BeApproximately(0, 1);
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

            var result = sut.Includes((int.MinValue, 0));

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

        [Test]
        public void AnyInterval_IsLesserThan_AnyNumberGreaterThanItsMax()
        {
            var sut = Interval.From(AnyPositiveNumber);

            var result = sut < AnyPositiveNumber + 1;

            result.Should().BeTrue();
        }
        
        [Test]
        public void AnyInterval_IsGreaterThan_AnyNumberLesserThanItsMin()
        {
            var sut = Interval.From(AnyPositiveNumber);

            var result = sut > AnyPositiveNumber - 1;

            result.Should().BeTrue();
        }

        [Test]
        public void NoInterval_IsLesserOrGreater_ThatAnyIncludedNumber()
        {
            var sut = Interval.Naturals;

            var resultLesser = sut < AnyPositiveNumber;
            var resultGreater = sut > AnyPositiveNumber;

            resultLesser.Should().BeFalse();
            resultGreater.Should().BeFalse();
        }
        
        [Test]
        public void IntervalOfNegativeNumbers_IsLesserThan_Zero()
        {
            var sut = Interval.From(-2, -1);

            var result = sut < 0;

            result.Should().BeTrue();
        }

        [Test]
        public void NaturalNumbersInterval_IsGreaterThan_Zero()
        {
            var sut = Interval.Naturals;

            var result = sut > 0;

            result.Should().BeTrue();
        }

        [Test]
        public void ZeroLenghtInterval_HasOneIteration_RelativeToItsNumber()
        {
            var sut = Interval.From(0);

            var result = sut.IterateOver();

            result.Should().HaveCount(1);
            result.Should().Contain(0);
        }
        
        [TestCase(0, 0, Description = "0-length interval")]
        [TestCase(-8, -1, Description = "Negative interval")] 
        [TestCase(-9, 0, Description = "Negative interval w/ 0")] 
        [TestCase(0, 1, Description = "Positive interval w/ 0")]
        [TestCase(90, 132, Description = "Positive interval")]
        public void AnyInterval_HasAsIterations_AsIntervalLenghtPlusOne(int min, int max)
        {
            var sut = Interval.From(min, max);

            var result = sut.IterateOver();

            result.Should().HaveCount(sut.Length + 1);
        }
    }
}