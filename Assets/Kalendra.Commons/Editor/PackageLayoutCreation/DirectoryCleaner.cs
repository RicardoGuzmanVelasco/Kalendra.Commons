using System.IO;

namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    public static class DirectoryCleaner
    {
        public static void Clean(string folderPath)
        {
            if(Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
            
            Directory.CreateDirectory(folderPath);
        }
    }
}