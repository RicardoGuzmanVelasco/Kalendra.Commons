using System.Collections.Generic;

namespace Kalendra.Commons.Editor
{
    public class AsmdefDeserialization
    {
        public string name = "";
        public string rootNamespace = "";
        
        public List<string> references = new List<string>();
        
        public List<string> includePlatforms = new List<string>();
        public List<string> excludePlatforms = new List<string>();
        
        public bool allowUnsafeCode;
        
        public bool overrideReferences;
        public List<string> precompiledReferences = new List<string>();
        
        public bool autoReferenced = true;
        public List<string> defineConstraints = new List<string>();

        public List<string> versionDefines = new List<string>();
        public bool noEngineReferences;
    }
}