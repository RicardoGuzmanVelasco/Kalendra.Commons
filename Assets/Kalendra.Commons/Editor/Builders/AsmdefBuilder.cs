using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

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

        public AsmdefBuilder WithUnsafeCode(bool allowUnsafeCode)
        {
            asmdef.AllowUnsafeCode = allowUnsafeCode;
            return this;
        }

        public AsmdefBuilder WithOverrideReferences(bool overrideReferences)
        {
            asmdef.OverrideReferences = overrideReferences;
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
        #endregion

        #region Builder implementation
        public Asmdef Build() => asmdef;

        public static implicit operator Asmdef(AsmdefBuilder builder) => builder.Build();
        public static implicit operator AsmdefDeserialization(AsmdefBuilder builder) => builder.Build();
        public static implicit operator string(AsmdefBuilder builder) => builder.Build();
        #endregion

        #region Support methods
        //TODO: it's a whole class!
        string BuildAsmdefContent()
        {
            var builder = new StringBuilder();
            // builder.AppendLine("{");
            //
            // builder.AppendLine(Pair(nameof(asmdef.Name), asmdef.Name) + ",");
            // builder.AppendLine(Pair(nameof(asmdef.RootNamespace), asmdef.RootNamespace ?? asmdef.Name) + ",");

            if(asmdef.IsTests && !asmdef.Name.Contains("Builders"))
                asmdef.References.AddRange(new[]
                {
                    "UnityEngine.TestRunner",
                    "UnityEditor.TestRunner",
                    "BoundfoxStudios.FluentAssertions"
                });
            builder.AppendLine(PairArray(nameof(asmdef.References), asmdef.References) + ",");

            // if(asmdef.IsEditor || asmdef.IsTests)
            // {
            //     builder.AppendLine(PairArray("includePlatforms", "Editor") + ",");
            //     builder.AppendLine(PairArray("excludePlatforms") + ",");
            // }
            
            // builder.AppendLine(Pair(nameof(asmdef.AllowUnsafeCode), asmdef.AllowUnsafeCode) + ",");

            if(asmdef.IsTests && !asmdef.Name.Contains("Builders"))
            {
                asmdef.OverrideReferences = true;
                asmdef.PrecompiledReferences.Add("nunit.framework.dll");
            }

            // builder.AppendLine(Pair(nameof(asmdef.OverrideReferences), asmdef.OverrideReferences) + ",");
            // builder.AppendLine(PairArray(nameof(asmdef.PrecompiledReferences), asmdef.PrecompiledReferences) + ",");

            if(asmdef.IsTests)
            {
                asmdef.AutoReferenced = false;
                asmdef.DefineConstraints.Add("UNITY_INCLUDE_TESTS");
            }
            // builder.AppendLine(Pair(nameof(asmdef.AutoReferenced), asmdef.AutoReferenced) + ",");
            // builder.AppendLine(PairArray(nameof(asmdef.DefineConstraints), asmdef.DefineConstraints) + ",");

            // builder.AppendLine(PairArray(nameof(asmdef.VersionDefines), asmdef.VersionDefines) + ",");
            // builder.AppendLine(Pair(nameof(asmdef.NoEngineReferences), asmdef.NoEngineReferences));

            // builder.AppendLine("}");

            return builder.ToString();
        }

        static string Pair(string key, bool value) => $"\"{key}\": {value.ToString().ToLower()}";
        static string Pair(string key, object value) => Pair(key, value.ToString());
        static string Pair(string key, string value) => $"\"{key}\": \"{value}\"";

        static string PairArray(string key, IEnumerable<string> values) => PairArray(key, values.ToArray());

        static string PairArray(string key, params string[] values)
        {
            var valuesArray = "[";

            foreach(var value in values)
                valuesArray += "\"" + value + "\",";

            if(valuesArray.Last() == ',')
                valuesArray = valuesArray.Remove(valuesArray.Length - 1);

            valuesArray += "]";

            return $"\"{key}\": {valuesArray}";
        }
        #endregion
    }
}