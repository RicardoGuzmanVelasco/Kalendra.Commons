using System;
using System.Linq;
using System.Text;

namespace Kalendra.Commons.Editor
{
    public class AsmdefBuilder
    {
        string name = "";
        /// <summary>
        /// If null, same as <see cref="name"/>.
        /// </summary>
        string rootNamespace = "";
        
        bool isEditor;
        bool isTests;

        bool allowUnsafeCode;
        bool overrideReferences;
        bool autoReferenced = true;
        bool noEngineReferences;

        #region Fluent API
        public AsmdefBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }
        
        public AsmdefBuilder WithRootNamespace(string rootNamespace)
        {
            this.rootNamespace = rootNamespace;
            return this;
        }
        public AsmdefBuilder WithRootNamespaceSameThanName()
        {
            rootNamespace = null;
            return this;
        }
        
        //TODO: might use a platforms string array.
        public AsmdefBuilder IsEditor(bool isEditor)
        {
            this.isEditor = isEditor;
            return this;
        }

        public AsmdefBuilder IsTests(bool isTests)
        {
            this.isTests = isTests;
            return this;
        }

        public AsmdefBuilder WithUnsafeCode(bool allowUnsafeCode)
        {
            this.allowUnsafeCode = allowUnsafeCode;
            return this;
        }

        public AsmdefBuilder WithOverrideReferences(bool overrideReferences)
        {
            this.overrideReferences = overrideReferences;
            return this;
        }

        public AsmdefBuilder WithAutoReferenced(bool autoReferenced)
        {
            this.autoReferenced = autoReferenced;
            return this;
        }

        public AsmdefBuilder WithEngineReferences(bool includeEngineReferences)
        {
            noEngineReferences = !includeEngineReferences;
            return this;
        }
        #endregion

        #region ObjectMother/FactoryMethods
        AsmdefBuilder() { }

        public static AsmdefBuilder New() => new AsmdefBuilder();
        #endregion

        #region Builder implementation
        //TODO: use embbeded Newtonsoft to autoserialize a defined Asmdef object.
        public string Build() => BuildAsmdefContent();

        public static implicit operator string(AsmdefBuilder builder) => builder.Build();
        #endregion

        #region Support methods
        string BuildAsmdefContent()
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");

            builder.AppendLine(Pair(nameof(name), name) + ",");
            builder.AppendLine(Pair(nameof(rootNamespace), rootNamespace ?? name) + ",");

            if(isEditor || isTests)
            {
                builder.AppendLine(PairArray("includePlatforms", "Editor") + ",");
                builder.AppendLine(PairArray("excludePlatforms") + ",");
            }
            
            builder.AppendLine(Pair(nameof(allowUnsafeCode), allowUnsafeCode) + ",");
            builder.AppendLine(Pair(nameof(overrideReferences), overrideReferences) + ",");
            builder.AppendLine(Pair(nameof(autoReferenced), autoReferenced) + ",");
            builder.AppendLine(Pair(nameof(noEngineReferences), noEngineReferences));
            
            builder.AppendLine("}");
            return builder.ToString();
        }

        static string Pair(string key, bool value) => $"\"{key}\": {value.ToString().ToLower()}";
        static string Pair(string key, object value) => Pair(key, value.ToString());
        static string Pair(string key, string value) => $"\"{key}\": \"{value}\"";

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