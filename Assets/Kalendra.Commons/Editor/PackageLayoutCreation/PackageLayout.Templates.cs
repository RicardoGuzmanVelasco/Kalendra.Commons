namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    public partial class PackageLayout
    {
        public static class Templates
        {
            public static PackageLayout UnityPackageLayout(string rootName)
            {
                var rootLayout = Folder(rootName);

                rootLayout.AddChild("Documentation~");
                rootLayout.AddChild("Editor"); //TODO: asmdef editor.
                rootLayout.AddChild("Runtime"); //TODO: receives architecture as children.

                var testsLayout = rootLayout.AddChild("Tests");
                testsLayout.AddChild("Builders"); //TODO: asmdef builders.
                testsLayout.AddChild("Editor"); //TODO: asmdef tests.
                testsLayout.AddChild("Runtime"); //TODO: asmdef tests.
            
                return rootLayout;
            }
        }
    }
}