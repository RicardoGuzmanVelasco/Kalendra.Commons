using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalendra.Commons.Editor
{
    public class AsmdefBuilder
    {
        readonly AsmdefDefinition asmdef;

        #region Fluent API
        public AsmdefBuilder WithName(string name)
        {
            asmdef.name = name;
            return this;
        }

        public AsmdefBuilder WithRootNamespace(string rootNamespace)
        {
            asmdef.rootNamespace = rootNamespace;
            return this;
        }

        public AsmdefBuilder WithRootNamespaceSameThanName()
        {
            asmdef.rootNamespace = null;
            return this;
        }

        //TODO: might use a platforms string array.
        public AsmdefBuilder IsEditor(bool isEditor)
        {
            asmdef.isEditor = isEditor;
            return this;
        }

        public AsmdefBuilder IsTests(bool isTests)
        {
            asmdef.isTests = isTests;
            return this;
        }

        public AsmdefBuilder WithReferences(params string[] references)
        {
            asmdef.references = references.ToList();
            return this;
        }

        public AsmdefBuilder WithUnsafeCode(bool allowUnsafeCode)
        {
            asmdef.allowUnsafeCode = allowUnsafeCode;
            return this;
        }

        public AsmdefBuilder WithOverrideReferences(bool overrideReferences)
        {
            asmdef.overrideReferences = overrideReferences;
            return this;
        }

        public AsmdefBuilder WithAutoReferenced(bool autoReferenced)
        {
            asmdef.autoReferenced = autoReferenced;
            return this;
        }

        public AsmdefBuilder WithEngineReferences(bool includeEngineReferences)
        {
            asmdef.noEngineReferences = !includeEngineReferences;
            return this;
        }
        #endregion

        #region ObjectMother/FactoryMethods
        AsmdefBuilder()
        {
            asmdef = new AsmdefDefinition();
        }

        public static AsmdefBuilder New() => new AsmdefBuilder();
        #endregion

        #region Builder implementation
        //TODO: use embbeded Newtonsoft to autoserialize a defined Asmdef object.
        public string Build() => BuildAsmdefContent();

        public static implicit operator string(AsmdefBuilder builder) => builder.Build();
        #endregion

        #region Support methods
        //TODO: it's a whole class!
        string BuildAsmdefContent()
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");

            builder.AppendLine(Pair(nameof(asmdef.name), asmdef.name) + ",");
            builder.AppendLine(Pair(nameof(asmdef.rootNamespace), asmdef.rootNamespace ?? asmdef.name) + ",");

            if(asmdef.isTests && !asmdef.name.Contains("Builders"))
                asmdef.references.AddRange(new[]
                {
                    "UnityEngine.TestRunner",
                    "UnityEditor.TestRunner",
                    "BoundfoxStudios.FluentAssertions"
                });
            builder.AppendLine(PairArray(nameof(asmdef.references), asmdef.references) + ",");

            if(asmdef.isEditor || asmdef.isTests)
            {
                builder.AppendLine(PairArray("includePlatforms", "Editor") + ",");
                builder.AppendLine(PairArray("excludePlatforms") + ",");
            }
            
            builder.AppendLine(Pair(nameof(asmdef.allowUnsafeCode), asmdef.allowUnsafeCode) + ",");

            if(asmdef.isTests && !asmdef.name.Contains("Builders"))
            {
                asmdef.overrideReferences = true;
                asmdef.precompiledReferences.Add("nunit.framework.dll");
            }

            builder.AppendLine(Pair(nameof(asmdef.overrideReferences), asmdef.overrideReferences) + ",");
            builder.AppendLine(PairArray(nameof(asmdef.precompiledReferences), asmdef.precompiledReferences) + ",");

            if(asmdef.isTests)
            {
                asmdef.autoReferenced = false;
                asmdef.defineConstraints.Add("UNITY_INCLUDE_TESTS");
            }
            builder.AppendLine(Pair(nameof(asmdef.autoReferenced), asmdef.autoReferenced) + ",");
            builder.AppendLine(PairArray(nameof(asmdef.defineConstraints), asmdef.defineConstraints) + ",");

            builder.AppendLine(PairArray(nameof(asmdef.versionDefines), asmdef.versionDefines) + ",");
            builder.AppendLine(Pair(nameof(asmdef.noEngineReferences), asmdef.noEngineReferences));

            builder.AppendLine("}");
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