using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Kalendra.Commons.Editor
{
    public static class PackageLayoutCreator
    {
        const string OrganizationName = "Kalendra";
        const string Hotkey = "%#&a";
        
        [MenuItem("Assets/Create/" + OrganizationName + "/Package Layout " + Hotkey)]
        public static void Create()
        {
            var currentFolderPath = FindCurrentFolderFromProjectWindow();
            AssertIfCurrentFolderMeetsBasePackageLayout(currentFolderPath);

            currentFolderPath.CreateSubfolder("Runtime/Domain");
            currentFolderPath.CreateSubfolder("Runtime/Infrastructure");
            
            currentFolderPath.CreateSubfolder("Tests/Editor");
            currentFolderPath.CreateSubfolder("Tests/Runtime");
            
            Debug.Log($"Created package layout from root {currentFolderPath}");
        }

        static void AssertIfCurrentFolderMeetsBasePackageLayout(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException("No folder selected?");
        }

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
        #endregion
    }
}