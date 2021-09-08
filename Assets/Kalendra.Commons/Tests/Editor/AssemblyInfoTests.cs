using System;
using FluentAssertions;
using Kalendra.Commons.Editor;
using NUnit.Framework;

namespace Kalendra.Commons.Tests.Editor
{
    public class AssemblyInfoTests
    {
        #region Fixture
        const string Something = "Something";

        const string BasicInfoUsing = "System.Reflection";
        const string ExposingInternalsUsing = "System.Runtime.CompilerServices";
        #endregion

        [Test]
        public void AssemblyInfo_HasNotUsings_ByDefault()
        {
            var sut = Build.AssemblyInfo();

            AssemblyInfoFile result = sut;

            result.Usings.Should().BeEmpty();
        }

        [Test]
        public void AssemblyInfo_HasUsings_WheneverInfoAttributeIsAdded()
        {
            var sut = Build.AssemblyInfo().WithTitle(Something);

            AssemblyInfoFile result = sut;

            result.Usings.Should().NotBeEmpty();
            result.Usings.Should().Contain(BasicInfoUsing);
        }
        
        [Test]
        public void AssemblyInfo_HasUsings_WheneverInternalsAreExposed()
        {
            var sut = Build.AssemblyInfo().WithInternalsVisibleTo(Something);

            AssemblyInfoFile result = sut;

            result.Usings.Should().NotBeEmpty();
            result.Usings.Should().Contain(ExposingInternalsUsing);
        }
        
        #region Serialization
        [Test]
        public void SerializedAssemblyInfo_WithUsings_ContainsUsingLine()
        {
            var sut = Build.AssemblyInfo().WithInternalsVisibleTo(Something).WithTitle(Something).Build();

            string result = sut;

            var expectedLine1 = $"using {ExposingInternalsUsing};";
            var expectedLine2 = $"using {BasicInfoUsing};";
            result.Should().Contain(expectedLine1);
            result.Should().Contain(expectedLine2);
        }
        
        [Test]
        public void SerializedAssemblyInfo_ContainsAllNotNullAttributes()
        {
            var sut = Build.AssemblyInfo().WithCompany(Something).WithCopyright("").Build();

            string result = sut;

            var expectedLine1 = $"[assembly: AssemblyCompany(\"{Something}\")]";
            result.Should().Contain(expectedLine1);
            result.Should().Contain("Copyright");
            result.Should().NotContain("Version");
            result.Should().NotContain("Title");
        }

        [Test]
        public void SerializedAssemblyInfo_ExposingInternals_ContainsInternalExpositionAttribute()
        {
            var sut = Build.AssemblyInfo().WithInternalsVisibleTo(Something).Build();

            string result = sut;

            var expectedLine = $"[assembly: InternalsVisibleTo(\"{Something}\")]";
            result.Should().Contain(expectedLine);  
        }
        #endregion
    }
}