using FluentAssertions;
using Kalendra.Commons.Editor.PackageLayoutCreation;
using Kalendra.Commons.Editor.PackageLayoutCreation.Builders;
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

        [Test]
        public void DefiningAsmdefWithPrecompiledReferences_AlsoEnablesOverrideReferencesFlag()
        {
            var sut = Build.Asmdef().WithPrecompiledReferences("Some");

            AsmdefDeserialization result = sut;

            result.overrideReferences.Should().BeTrue();
        }

        [Test]
        public void DefiningAsmdef_DisablesOverrideReferencesFlag_ByDefault()
        {
            var sut = Build.Asmdef();

            AsmdefDeserialization result = sut;

            result.overrideReferences.Should().BeFalse();
        }

        [Test]
        public void DefiningAsmdefWithPrecompiledReferences_IncludesThoseReferences()
        {
            var sut = Build.Asmdef().WithPrecompiledReferences("Ref1", "Ref2");

            AsmdefDeserialization result = sut;

            result.precompiledReferences.Should().Contain("Ref1");
            result.precompiledReferences.Should().Contain("Ref2");
        }

        [Test]
        public void DefiningAsmdefWithPrecompiledReferences_IfAlsoIsTests_AlsoIncludesTestingReferences()
        {
            var sut = Build.Asmdef().WithPrecompiledReferences("Ref").IsTests(true);

            AsmdefDeserialization result = sut;

            result.precompiledReferences.Should().Contain("nunit.framework.dll");
            result.precompiledReferences.Should().Contain("NSubstitute.dll");
        }
        #endregion

        #region Constraints
        [Test]
        public void TestsAsmdef_IncludesTestingDefineConstraint()
        {
            var sut = Build.Asmdef().IsTests(true);

            AsmdefDeserialization result = sut;

            result.defineConstraints.Should().Contain("UNITY_INCLUDE_TESTS");
        }

        [Test]
        public void TestsAsmdef_DisablesAutoReferencedFlag()
        {
            var sut = Build.Asmdef().IsTests(true);

            AsmdefDeserialization result = sut;

            result.autoReferenced.Should().BeFalse();
        }

        [Test]
        public void AutoReferencedFlag_IsEnabled_ByDefault()
        {
            var sut = Build.Asmdef();

            AsmdefDeserialization result = sut;

            result.autoReferenced.Should().BeTrue();
        }
        #endregion
        
        #region Infer
        [Test]
        public void Infer_EditorAsmdef_IsSameThanEditorAsmdef()
        {
            AsmdefDeserialization expected = Build.Asmdef().WithName("...Editor...").IsEditor(true);
            var sut = Build.Asmdef().WithName("...Editor...").InferFromName();

            AsmdefDeserialization result = sut;
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Infer_TestsAsmdef_IsSameThanTestsAsmdef()
        {
            AsmdefDeserialization expected = Build.Asmdef().WithName("...Tests...").IsTests(true);
            var sut = Build.Asmdef().WithName("...Tests...").InferFromName();

            AsmdefDeserialization result = sut;
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Infer_BuildersAsmdef_IsSameThanBuildersAsmdef()
        {
            AsmdefDeserialization expected = Build.Asmdef().WithName("...Builders...").IsBuilders(true);
            var sut = Build.Asmdef().WithName("...Builders...").InferFromName();

            AsmdefDeserialization result = sut;
            
            result.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region Serialization
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
            var expectedAutoReferenced = JsonBuild.JsonPair<bool>().WithName("autoReferenced").WithValue(true);
            var expectedNoEngineReferences = JsonBuild.JsonPair<bool>().WithName("noEngineReferences").WithValue(false);
            var sut = Build
                .Asmdef()
                .WithUnsafeCode(expectedUnsafeCode.Value)
                .WithAutoReferenced(expectedAutoReferenced.Value)
                .WithEngineReferences(!expectedNoEngineReferences.Value);

            string jsonResult = sut;

            jsonResult.Should().Contain(expectedUnsafeCode);
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