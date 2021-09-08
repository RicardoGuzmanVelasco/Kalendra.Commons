using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Kalendra.Commons.Editor.PackageLayoutCreation.Builders;
using UnityEditor;
using UnityEngine;

namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    public static class PackageLayoutCreatorInspector
    {
        const string DefaultOrganizationName = "Kalendra";
        const string DefaultVersion = "0.0.1";
        const string DefaultCopyrightClaim = "(C) 2021 RGV";
        

        #region Editor/Inspector
        const string HotkeyClean = "%#&q";
        const string HotkeyCreate = "%#&a";

        [MenuItem("Assets/Create/" + DefaultOrganizationName + "/Clean package Layout " + HotkeyClean)]
        public static void Clean()
        {
            var currentFolderPath = FindCurrentFolderFromProjectWindow();
            
            CleanPackageLayoutInFolder(currentFolderPath);
        }

        [MenuItem("Assets/Create/" + DefaultOrganizationName + "/Package Layout " + HotkeyCreate)]
        public static void Create()
        {
            var currentFolderPath = FindCurrentFolderFromProjectWindow();
            CleanPackageLayoutInFolder(currentFolderPath);
            
            CreatePackageLayoutInFolder(currentFolderPath);
        }
        #endregion
        
        static void CleanPackageLayoutInFolder(string folderPath)
        {
            DirectoryCleaner.Clean(folderPath);

            Debug.LogWarning($"Cleaned package layout from root {folderPath}");
            Recompile();
        }

        static void CreatePackageLayoutInFolder(string folderPath)
        {
            //TODO: to policy.
            folderPath.AssertMeetsBasePackageLayout();

            folderPath.CreateSubfolder("Documentation~");
            
            //TODO: to policy.
            folderPath.CreateFolderWithAssembly("Editor");
            
            //TODO: to policy.
            folderPath.CreateFolderWithAssembly("Runtime/Domain");
            folderPath.CreateFolderWithAssembly("Runtime/Infrastructure");

            //TODO: to policy.
            folderPath.CreateFolderWithAssembly("Tests/Editor");
            folderPath.CreateFolderWithAssembly("Tests/Runtime");
            folderPath.CreateFolderWithAssembly("Tests/Builders");

            Debug.Log($"Created package layout from root {folderPath}");
            Recompile();
        }

        #region Fake extensions
        static void CreateFolderWithAssembly(this string path, string folder)
        {
            path.CreateSubfolder(folder);
            var folderPath = path.ConcatPath(folder);

            var fileName = $"{path.FilenameFromPath()}.{folder.Replace("/", ".")}";
            CreateAsmdef(fileName, folderPath);
            CreateAssemblyInfoFile(fileName, folderPath);
        }

        static void CreateAsmdef(string name, string path)
        {
            name = name.IgnoreLayers("Runtime"); //TODO: to Asmdef responsability.

            Asmdef asmdefContent = Build
                .Asmdef()
                .WithName(name)
                .WithRootNamespaceSameThanName()
                .InferFromName();
            
            File.WriteAllText($"{path}/{name}.asmdef", asmdefContent);
        }

        static string IgnoreLayers(this string path, params string[] layersToIgnore)
        {
            foreach(var layer in layersToIgnore)
                path = path.Replace(layer, "");

            while(path.Contains(".."))
                path = path.Replace("..", ".");
            path = path.Trim('.');
            
            return path;
        }

        static void CreateAssemblyInfoFile(string name, string path)
        {
            var nameElements = name.Split('.');

            var inferredCompany = nameElements.Length >= 3 ? nameElements[0] : null;
            var inferredProduct = nameElements.Length >= 3 ? nameElements[1] : nameElements.First();

            var companyAndProductLength = (inferredCompany + "." + inferredProduct + ".").Length;
            var inferredTitle = name.Substring(companyAndProductLength);

            AssemblyInfoFile content = Build.AssemblyInfo()
                .WithCompany(inferredCompany)
                .WithProduct(inferredProduct)
                .WithTitle(inferredTitle)
                .WithVersion(DefaultVersion)
                .WithCopyright(DefaultCopyrightClaim);
            
            File.WriteAllText($"{path}/AssemblyInfo.cs", content);
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