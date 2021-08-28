using System.Collections.Generic;

namespace Kalendra.Commons.Editor
{
    public class AsmdefDefinition
    {
        public string name = "";

        /// <summary>
        /// If null, same as <see cref="name"/>.
        /// </summary>
        public string rootNamespace = "";
 
        public bool isEditor;
        public bool isTests;
        
        public List<string> references = new List<string>();
        
        public bool allowUnsafeCode;
        
        public bool overrideReferences;
        public List<string> precompiledReferences = new List<string>();
        
        public bool autoReferenced = true;
        public List<string> defineConstraints = new List<string>();

        public List<string> versionDefines = new List<string>();
        public bool noEngineReferences;
    }
}