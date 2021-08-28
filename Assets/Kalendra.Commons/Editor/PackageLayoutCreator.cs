using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Kalendra.Commons.Editor
{
    public static class PackageLayoutCreator
    {
        #region Editor/Inspector
        const string OrganizationName = "Kalendra";
        
        const string HotkeyCreate = "%#&a";
        const string HotkeyClean = "%#&q";

        [MenuItem("Assets/Create/" + OrganizationName + "/Clean package Layout " + HotkeyClean)]
        public static void Clean()
        {
            var currentFolderPath = FindCurrentFolderFromProjectWindow();
            
            CleanPackageLayoutInFolder(currentFolderPath);
        }

        [MenuItem("Assets/Create/" + OrganizationName + "/Package Layout " + HotkeyCreate)]
        public static void Create()
        {
            var currentFolderPath = FindCurrentFolderFromProjectWindow();
            CleanPackageLayoutInFolder(currentFolderPath);
            
            CreatePackageLayoutInFolder(currentFolderPath);
        }
        #endregion
        
        static void CleanPackageLayoutInFolder(string folderPath)
        {
            if(Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
            Directory.CreateDirectory(folderPath);

            Debug.Log($"Cleaned package layout from root {folderPath}");
            Recompile();
        }

        static void CreatePackageLayoutInFolder(string folderPath)
        {
            folderPath.AssertMeetsBasePackageLayout();

            folderPath.CreateFolderWithAssembly("Runtime/Domain");
            folderPath.CreateFolderWithAssembly("Runtime/Infrastructure");

            folderPath.CreateFolderWithAssembly("Tests/Editor");
            folderPath.CreateFolderWithAssembly("Tests/Runtime");

            Debug.Log($"Created package layout from root {folderPath}");
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
            var isEditorAssembly = asmdefName.Contains("Editor") || asmdefPath.Contains("Editor");
            var isTestAssembly = asmdefName.Contains("Tests") || asmdefPath.Contains("Tests");
            
            var asmdefContent = Build
                                .Asmdef()
                                .WithName(asmdefName)
                                .IsEditor(isEditorAssembly)
                                .IsTests(isTestAssembly);
            
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