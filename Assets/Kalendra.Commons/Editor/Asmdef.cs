using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Kalendra.Commons.Editor
{
    /// <summary>
    /// Does not represent an asmdef file — that's <see cref="AsmdefDeserialization"/>.
    /// Represents instead how someone wants a desired asmdef. It's also able to serialize it.
    /// TODO: support for external change of precompiled references (dlls).
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

        public bool OverrideReferences
        {
            get => asmdefFileContent.overrideReferences;
            set => asmdefFileContent.overrideReferences = value;
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

        #region Conversions
        public static implicit operator string(Asmdef source)
        {
            var asmdef = ToDeserializedAsmdef(source);
            var jsonIgnoreNulls = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
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
                overrideReferences = source.OverrideReferences,
                precompiledReferences = source.PrecompiledReferences,
                autoReferenced = source.AutoReferenced,
                defineConstraints = source.DefineConstraints,
                versionDefines = source.VersionDefines,
                noEngineReferences = source.NoEngineReferences
            };

            HandlePlatforms(source, asmdef);
            HandleReferences(source, asmdef);

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
            
            target.precompiledReferences.Add("nunit.framework.dll");
        }
        #endregion
    }

    public static class AsmdefExtensions
    {
        public static bool IsEditorOnly(this Asmdef source) => source.IsBuilders || source.IsTests || source.IsEditor;

        public static bool SpecifiesAnyPlatform(this Asmdef source) => source.IncludedPlatforms.Any() || source.ExcludedPlatforms.Any();
    }
}