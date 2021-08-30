using System;
using FluentAssertions;
using Kalendra.Commons.Editor;
using NUnit.Framework;

using JsonBuild = Kalendra.Commons.Runtime.Domain.Builders.Build;

namespace Kalendra.Commons.Tests.Editor
{
    public class AsmdefTests
    {
        #region Platforms
        [Test]
        public void NotEditorAsmdef_NoPlatformsField()
        {
            var sut = Build.Asmdef().IsEditor(false);

            string result = sut;

            result.Should().NotContainAny("includePlatforms", "excludePlatforms");
        }
        
        [Test]
        public void EditorAsmdef_ThenJustEditorPlatform()
        {
            var sut = Build.Asmdef().IsEditor(true);

            AsmdefDeserialization result = sut;

            result.includePlatforms.Should().ContainSingle("Editor");
            result.excludePlatforms.Should().BeEmpty();
        }

        [Test]
        public void TestsAsmdef_ThenIsSameThanEditorAsmdef()
        {
            var sut = Build.Asmdef().IsTests(true);

            AsmdefDeserialization result = sut;
            
            AsmdefDeserialization expected = Build.Asmdef().IsEditor(true);
            result.includePlatforms.Should().BeEquivalentTo(expected.includePlatforms);
            result.excludePlatforms.Should().BeEquivalentTo(expected.excludePlatforms);
        }
        
        [Test]
        public void BuildersAsmdef_IncludesSamePlatformsThanTestsAsmdef()
        {
            var sut = Build.Asmdef().IsBuilders(true);

            AsmdefDeserialization result = sut;
            
            AsmdefDeserialization expected = Build.Asmdef().IsTests(true);
            result.includePlatforms.Should().BeEquivalentTo(expected.includePlatforms);
            result.excludePlatforms.Should().BeEquivalentTo(expected.excludePlatforms);
        }

        [Test]
        public void BuilderButNotTestsAsmdef_WhenToDeserialized_ThrowsException()
        {
        }

        [Test]
        public void NotEditorAsmdef_WithExcludedPlatform_ThenJustHasExcludedPlatforms()
        {
            var sut = Build.Asmdef().IsEditor(false).WithExcludedPlatforms("Test");

            AsmdefDeserialization result = sut;

            result.includePlatforms.Should().BeEmpty();
            result.excludePlatforms.Should().ContainSingle("Test");
        }
        
        [Test]
        public void NotEditorAsmdef_WithIncludedPlatform_ThenJustHasIncludedPlatforms()
        {
            var sut = Build.Asmdef().IsEditor(false).WithIncludedPlatforms("Test");

            AsmdefDeserialization result = sut;

            result.includePlatforms.Should().ContainSingle("Test");
            result.excludePlatforms.Should().BeEmpty();
        }
        #endregion
        
        #region References
        [Test]
        public void TestsAsmdef_IncludesTestingReferences()
        {
            var sut = Build.Asmdef().IsTests(true);

            AsmdefDeserialization result = sut;

            result.references.Should().Contain("UnityEngine.TestRunner");
            result.references.Should().Contain("UnityEditor.TestRunner");
            result.references.Should().Contain("BoundfoxStudios.FluentAssertions");
        }
        
        [Test]
        public void BuildersAsmdef_DoesNotIncludeTestingReferences()
        {
            var sut = Build.Asmdef().IsTests(false).IsBuilders(true);

            AsmdefDeserialization result = sut;

            result.references.Should().NotContain("UnityEngine.TestRunner");
            result.references.Should().NotContain("UnityEditor.TestRunner");
            result.references.Should().NotContain("BoundfoxStudios.FluentAssertions");
        }

        [Test]
        public void DefiningAsmdefWithReferences_IncludesThoseReferences()
        {
            var sut = Build.Asmdef().WithReferences("Ref1", "Ref2");

            AsmdefDeserialization result = sut;

            result.references.Should().Contain("Ref1");
            result.references.Should().Contain("Ref2");
        }

        [Test]
        public void DefiningAsmdefWithReferences_IfAlsoIsTests_AlsoIncludesTestingReferences()
        {
            var sut = Build.Asmdef().WithReferences("Ref").IsTests(true);

            AsmdefDeserialization result = sut;

            result.references.Should().Contain("Ref");
            result.references.Should().Contain("UnityEngine.TestRunner");
            result.references.Should().Contain("UnityEditor.TestRunner");
            result.references.Should().Contain("BoundfoxStudios.FluentAssertions");
        }
        
        [Test]
        public void TestsAsmdef_IncludesTestingPrecompiledReferences()
        {
            var sut = Build.Asmdef().IsTests(true);

            AsmdefDeserialization result = sut;

            result.precompiledReferences.Should().Contain("nunit.framework.dll");
        }
        #endregion
        
        #region Sanity
        [Test]
        public void Serialization_SavesName()
        {
            var expected = JsonBuild.JsonPair<string>().WithName("name").WithValue("Test!");
            var sut = Build.Asmdef().WithName(expected.Value);

            string jsonResult = sut;

            jsonResult.Should().Contain(expected);
        }

        [Test]
        public void Serialization_RootNamespace_IsEmptyByDefault()
        {
            var sut = Build.Asmdef();

            AsmdefDeserialization result = sut;

            result.rootNamespace.Should().BeEmpty();
        }
        
        [Test]
        public void Serialization_SavesRootNamespace()
        {
            var expected = JsonBuild.JsonPair<string>().WithName("rootNamespace").WithValue("Test!");
            var sut = Build.Asmdef().WithRootNamespace(expected.Value);

            string jsonResult = sut;

            jsonResult.Should().Contain(expected);
        }        

        [Test]
        public void Serialization_WhenLinkedNamespaceAndName_SavesRootNamespaceAsName()
        {
            var expected = JsonBuild.JsonPair<string>().WithName("rootNamespace").WithValue("Test!");
            var sut = Build.Asmdef().WithName(expected.Value).WithRootNamespaceSameThanName();

            string jsonResult = sut;

            jsonResult.Should().Contain(expected);
        }
        
        [Test]
        public void Serialization_SavesRemainingFlags()
        {
            var expectedUnsafeCode = JsonBuild.JsonPair<bool>().WithName("allowUnsafeCode").WithValue(true);
            var expectedOverrideReferences = JsonBuild.JsonPair<bool>().WithName("overrideReferences").WithValue(false);
            var expectedAutoReferenced = JsonBuild.JsonPair<bool>().WithName("autoReferenced").WithValue(true);
            var expectedNoEngineReferences = JsonBuild.JsonPair<bool>().WithName("noEngineReferences").WithValue(false);
            var sut = Build
                .Asmdef()
                .WithUnsafeCode(expectedUnsafeCode.Value)
                .WithOverrideReferences(expectedOverrideReferences.Value)
                .WithAutoReferenced(expectedAutoReferenced.Value)
                .WithEngineReferences(!expectedNoEngineReferences.Value);

            string jsonResult = sut;

            jsonResult.Should().Contain(expectedUnsafeCode);
            jsonResult.Should().Contain(expectedOverrideReferences);
            jsonResult.Should().Contain(expectedAutoReferenced);
            jsonResult.Should().Contain(expectedNoEngineReferences);
        }

        [Test]
        public void Serialization_SavesSomeElementInLists()
        {
            var sut = Build.Asmdef().WithReferences("Ref1", "Ref2");

            string jsonResult = sut;

            jsonResult.Should().Contain("Ref1");
            jsonResult.Should().Contain("Ref2");
        }
        #endregion
    }
}