using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalendra.Commons.Editor
{
    public class AsmdefBuilder
    {
        readonly Asmdef asmdef;

        #region Fluent API
        public AsmdefBuilder WithName(string name)
        {
            asmdef.Name = name;
            return this;
        }

        public AsmdefBuilder WithRootNamespace(string rootNamespace)
        {
            asmdef.RootNamespace = rootNamespace;
            return this;
        }

        public AsmdefBuilder WithRootNamespaceSameThanName()
        {
            asmdef.RootNamespace = null;
            return this;
        }

        public AsmdefBuilder IsEditor(bool isEditor)
        {
            asmdef.IsEditor = isEditor;
            return this;
        }

        public AsmdefBuilder IsTests(bool isTests)
        {
            asmdef.IsTests = isTests;
            return this;
        }

        public AsmdefBuilder IsBuilders(bool isBuilders)
        {
            asmdef.IsBuilders = isBuilders;
            return this;
        }

        public AsmdefBuilder WithIncludedPlatforms(params string[] includedPlatforms)
        {
            asmdef.IncludedPlatforms = includedPlatforms.ToList();
            return this;
        }

        public AsmdefBuilder WithExcludedPlatforms(params string[] excludedPlatforms)
        {
            asmdef.ExcludedPlatforms = excludedPlatforms.ToList();
            return this;
        }
        
        public AsmdefBuilder WithReferences(params string[] references)
        {
            asmdef.References = references.ToList();
            return this;
        }

        public AsmdefBuilder WithPrecompiledReferences(params string[] dlls)
        {
            asmdef.PrecompiledReferences = dlls.ToList();
            return this;
        }
        
        public AsmdefBuilder WithUnsafeCode(bool allowUnsafeCode)
        {
            asmdef.AllowUnsafeCode = allowUnsafeCode;
            return this;
        }

        public AsmdefBuilder WithAutoReferenced(bool autoReferenced)
        {
            asmdef.AutoReferenced = autoReferenced;
            return this;
        }

        public AsmdefBuilder WithEngineReferences(bool includeEngineReferences)
        {
            asmdef.NoEngineReferences = !includeEngineReferences;
            return this;
        }
        #endregion

        #region ObjectMother/FactoryMethods
        AsmdefBuilder()
        {
            asmdef = new Asmdef();
        }

        public static AsmdefBuilder New() => new AsmdefBuilder();

        public AsmdefBuilder InferFromName()
        {
            IsEditor(asmdef.Name.Contains("Editor"));
            IsTests(asmdef.Name.Contains("Tests"));
            IsBuilders(asmdef.Name.Contains("Builders"));

            return this;
        }
        #endregion

        #region Builder implementation
        public Asmdef Build() => asmdef;

        public static implicit operator Asmdef(AsmdefBuilder builder) => builder.Build();
        public static implicit operator AsmdefDeserialization(AsmdefBuilder builder) => builder.Build();
        public static implicit operator string(AsmdefBuilder builder) => builder.Build();
        #endregion
    }
}