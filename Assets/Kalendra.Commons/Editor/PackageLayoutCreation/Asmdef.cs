using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    /// <summary>
    /// Does not represent an asmdef file — that's <see cref="AsmdefDeserialization"/>.
    /// Represents instead how someone wants a desired asmdef. It's also able to serialize it.
    /// </summary>
    public class Asmdef
    {
        readonly AsmdefDeserialization asmdefFileContent = new AsmdefDeserialization();
        
        #region Properties
        public string Name
        {
            get => asmdefFileContent.name;
            set => asmdefFileContent.name = value;
        }

        /// <summary>
        /// If null, same than Name.
        /// </summary>
        public string RootNamespace
        {
            get => asmdefFileContent.rootNamespace ?? asmdefFileContent.name;
            set => asmdefFileContent.rootNamespace = value;
        }

        public bool IsEditor { get; set; }
        public bool IsTests { get; set; }
        public bool IsBuilders { get; set; }

        public List<string> IncludedPlatforms
        {
            get => asmdefFileContent.includePlatforms;
            set => asmdefFileContent.includePlatforms = value;
        }
        public List<string> ExcludedPlatforms
        {
            get => asmdefFileContent.excludePlatforms;
            set => asmdefFileContent.excludePlatforms = value;
        }

        public List<string> References
        {
            get => asmdefFileContent.references;
            set => asmdefFileContent.references = value;
        }
        
        public bool AllowUnsafeCode
        {
            get => asmdefFileContent.allowUnsafeCode;
            set => asmdefFileContent.allowUnsafeCode = value;
        }

        public List<string> PrecompiledReferences
        {
            get => asmdefFileContent.precompiledReferences;
            set => asmdefFileContent.precompiledReferences = value;
        }

        public bool AutoReferenced
        {
            get => asmdefFileContent.autoReferenced;
            set => asmdefFileContent.autoReferenced = value;
        }

        public List<string> DefineConstraints
        {
            get => asmdefFileContent.defineConstraints;
            set => asmdefFileContent.defineConstraints = value;
        }

        public List<string> VersionDefines
        {
            get => asmdefFileContent.versionDefines;
            set => asmdefFileContent.versionDefines = value;
        }

        public bool NoEngineReferences
        {
            get => asmdefFileContent.noEngineReferences;
            set => asmdefFileContent.noEngineReferences = value;
        }
        #endregion
        
        public bool IsEditorOnly() => IsBuilders || IsTests || IsEditor;
        
        public bool SpecifiesAnyPlatform() => IncludedPlatforms.Any() || ExcludedPlatforms.Any();
        
        #region Conversions
        public static implicit operator string(Asmdef source)
        {
            var asmdef = ToDeserializedAsmdef(source);
            var jsonIgnoreNulls = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            
            return JsonConvert.SerializeObject(asmdef, Formatting.Indented, jsonIgnoreNulls);
        }

        public static implicit operator AsmdefDeserialization(Asmdef source) => ToDeserializedAsmdef(source);
        static AsmdefDeserialization ToDeserializedAsmdef(Asmdef source)
        {
            var asmdef = new AsmdefDeserialization
            {
                name = source.Name,
                rootNamespace = source.RootNamespace,
                includePlatforms = source.IncludedPlatforms,
                excludePlatforms = source.ExcludedPlatforms,
                references = source.References,
                allowUnsafeCode = source.AllowUnsafeCode,
                precompiledReferences = source.PrecompiledReferences,
                autoReferenced = source.AutoReferenced,
                defineConstraints = source.DefineConstraints,
                versionDefines = source.VersionDefines,
                noEngineReferences = source.NoEngineReferences
            };

            HandlePlatforms(source, asmdef);
            HandleReferences(source, asmdef);
            HandlePrecompiledReferences(source, asmdef);
            HandleConstraints(source, asmdef);

            return asmdef;
        }

        static void HandlePlatforms(Asmdef source, AsmdefDeserialization target)
        {
            if(source.IsEditorOnly())
                target.includePlatforms = new List<string> { "Editor" };
            else if(!source.SpecifiesAnyPlatform())
            {
                target.includePlatforms = null;
                target.excludePlatforms = null;
            }
        }

        static void HandleReferences(Asmdef source, AsmdefDeserialization target)
        {
            if(!source.IsTests)
                return;
            
            target.references.Add("UnityEngine.TestRunner");
            target.references.Add("UnityEditor.TestRunner");
            target.references.Add("BoundfoxStudios.FluentAssertions");
        }
        
        static void HandlePrecompiledReferences(Asmdef source, AsmdefDeserialization target)
        {
            if(source.IsTests)
            {
                target.precompiledReferences.Add("nunit.framework.dll");
                target.precompiledReferences.Add("NSubstitute.dll");
            }

            target.overrideReferences = target.precompiledReferences.Any();
        }

        static void HandleConstraints(Asmdef source, AsmdefDeserialization target)
        {
            if(source.IsTests)
                target.defineConstraints.Add("UNITY_INCLUDE_TESTS");

            target.autoReferenced = !target.defineConstraints.Any();
        }
        #endregion
    }
}