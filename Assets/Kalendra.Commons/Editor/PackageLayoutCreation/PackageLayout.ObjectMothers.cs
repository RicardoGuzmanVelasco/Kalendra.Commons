using System;

namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    public partial class PackageLayout
    {
        static PackageLayout FolderFromParent(string name, PackageLayout parent) => parent.AddChild(name);
        PackageLayout AddChild(string name)
        {
            var child = Folder(name);
            AddChild(child);
            
            return child;
        }
        void AddChild(PackageLayout child)
        {
            children.Add(child);
            child.Parent = this;
        }
        
        static PackageLayout Folder(string name) => new PackageLayout { Name = name };

        public PackageLayout AttachAssembly(Asmdef asmdef)
        {
            Assembly = asmdef;

            if(string.IsNullOrEmpty(asmdef.Name))
                asmdef.Name = AssemblyNameFromPath;
            
            return this;
        }
    }
}