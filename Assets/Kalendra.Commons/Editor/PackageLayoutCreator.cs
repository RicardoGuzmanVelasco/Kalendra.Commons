using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Kalendra.Commons.Editor
{
    public static class PackageLayoutCreator
    {
        const string OrganizationName = "Kalendra";
        const string hotkeyCreate = "%#&a";
        const string hotkeyClean = "%#&q";

        [MenuItem("Assets/Create/" + OrganizationName + "/Clean package Layout " + hotkeyClean)]
        public static void Clean()
        {
            var currentFolderPath = FindCurrentFolderFromProjectWindow();
            currentFolderPath.AssertMeetsBasePackageLayout();
            
            if(Directory.Exists(currentFolderPath))
                Directory.Delete(currentFolderPath, true);
            Directory.CreateDirectory(currentFolderPath);
            
            Recompile();
        }
        
        [MenuItem("Assets/Create/" + OrganizationName + "/Package Layout " + hotkeyCreate)]
        public static void Create()
        {
            var currentFolderPath = FindCurrentFolderFromProjectWindow();
            currentFolderPath.AssertMeetsBasePackageLayout();

            currentFolderPath.CreateFolderWithAssembly("Runtime/Domain");
            currentFolderPath.CreateFolderWithAssembly("Runtime/Infrastructure");
            
            currentFolderPath.CreateFolderWithAssembly("Tests/Editor");
            currentFolderPath.CreateFolderWithAssembly("Tests/Runtime");
            
            Debug.Log($"Created package layout from root {currentFolderPath}");
            Recompile();
        }

        #region Fake extensions
        static void CreateFolderWithAssembly(this string path, string folder)
        {
            path.CreateSubfolder(folder);
            var assemblyFolderPath = path.ConcatPath(folder);

            var assemblyName = folder.Replace("/", ".");
            
            CreateAsmdef(assemblyName, assemblyFolderPath);
        }

        static void CreateAsmdef(string asmdefName, string asmdefPath)
        {
            var isEditor = asmdefName.Contains("Editor") || asmdefPath.Contains("Editor");
            
            var asmdefContent = Build.Asmdef().WithName(asmdefName).IsEditor(isEditor);
            
            File.WriteAllText($"{asmdefPath}/{asmdefName}.asmdef", asmdefContent);
        }
        #endregion

        #region Support methods
        /// <remarks>
        /// Use an obsolete way to find selection. Better use another way.
        /// </remarks>
        static string FindCurrentFolderFromProjectWindow()
        {
            #pragma warning disable 618
            var lastClickedObject = EditorUtility.GetAssetPath(Selection.activeObject);
            #pragma warning restore 618
            if(!string.IsNullOrWhiteSpace(lastClickedObject))
                return lastClickedObject;
            
            var findActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            var currentActiveObject = findActiveFolderPath?.Invoke(null, null).ToString();

            return currentActiveObject;
        }
        
        static void AssertMeetsBasePackageLayout(this string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException("No folder selected?");
        }

        static void Recompile() => AssetDatabase.Refresh();
        #endregion
    }
}