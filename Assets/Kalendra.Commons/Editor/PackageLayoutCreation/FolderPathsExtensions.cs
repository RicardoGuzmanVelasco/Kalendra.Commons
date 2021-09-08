using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    public static class FolderPathsExtensions
    {
        static readonly char Separator = Path.AltDirectorySeparatorChar;
        
        public static string FilenameFromPath(this string path) => path.Split(Separator).Last();
        public static string ConcatPath(this string path, string newPath) => $"{path}{Separator}{newPath}";
        public static bool IsFolder(this string path) => AssetDatabase.IsValidFolder(path);

        public static bool HasFolder(this string path, string subfolderPath)
        {
            if(!path.IsFolder())
                throw new ArgumentException($"{path} is not a valid folder");
            
            if(string.IsNullOrWhiteSpace(subfolderPath))
                throw new ArgumentException("Empty subfolder path");
            
            return AssetDatabase.IsValidFolder(path.ConcatPath(subfolderPath));
        }

        public static void CreateSubfolder(this string parentPath, string newFolderName)
        {
            if(!parentPath.IsFolder())
                throw new InvalidOperationException($"{parentPath} is not a valid folder.");
            
            var newFolderPath = newFolderName.Split(Separator);
            newFolderName = newFolderPath.Last();
            var subFoldersNames = newFolderPath.Take(newFolderPath.Length - 1);

            foreach(var subfolder in subFoldersNames.Where(s => !parentPath.HasFolder(s)))
                parentPath.CreateSubfolder(subfolder);
            
            if(newFolderPath.Length > 1)
                parentPath = subFoldersNames.Aggregate(parentPath, (p, s) => p.ConcatPath(s));

            var createdFolderGuid = AssetDatabase.CreateFolder(parentPath, newFolderName);
            if(string.IsNullOrEmpty(createdFolderGuid))
                throw new InvalidOperationException($"Couldn't create folder {newFolderName} in {parentPath}");
        }
    }
}