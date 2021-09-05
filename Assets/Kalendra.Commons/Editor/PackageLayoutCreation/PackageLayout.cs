using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalendra.Commons.Editor
{
    public class PackageLayout
    {
        #region FactoryMethods/ObjectMothers
        public static PackageLayout Documentation => Folder("Documentation~");
        
        public static PackageLayout Folder(string name) => new PackageLayout { Name = name };
        public static PackageLayout FolderWithAssembly(string name, Asmdef asmdef) => new PackageLayout { Name = name, Assembly = asmdef };
        #endregion

        string Name { get; set; }
        public bool IgnoreAsChildrenNamePart { get; set; }
        
        Asmdef Assembly { get; set; }

        readonly List<PackageLayout> dependencies = new List<PackageLayout>();
        public IEnumerable<PackageLayout> Dependencies => dependencies;
        public void AddDependency(PackageLayout dependency) => dependencies.Add(dependency);

        readonly List<PackageLayout> children = new List<PackageLayout>();
        IEnumerable<PackageLayout> Children => children;
        void AddChild(PackageLayout child) => children.Add(child);

        #region Formatting members
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Name);
            foreach(var indentedChild in Children.Select(l => IndentChild(l.ToString())))
                builder.Append("\t" + indentedChild);
            
            return builder.ToString();
        }

        static string IndentChild(string child)
        {
            var childLines = child.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            var indentedChild = childLines.Aggregate("", (r, line) => $"{r}\n\t{line}");
            return indentedChild;
        }
        #endregion
        
        #region Nested members
        public static class Templates
        {
            public static PackageLayout UnityPackageLayout(string rootName)
            {
                var rootLayout = Folder(rootName);

                rootLayout.AddChild(Documentation);
            
                var testLayout = Folder("Tests");
                rootLayout.AddChild(testLayout);
            
                testLayout.AddChild(Folder("Runtime"));
                testLayout.AddChild(Folder("Editor"));
            
                return rootLayout;
            }
        }
        #endregion
    }
}