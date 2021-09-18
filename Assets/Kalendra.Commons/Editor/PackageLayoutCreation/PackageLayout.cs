using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalendra.Commons.Editor.PackageLayoutCreation.Builders;

namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    public partial class PackageLayout
    {
        string Name { get; set; }
        public bool IgnoreAsChildrenNamePart { get; set; }

        string AssemblyNameFromPath
        {
            get
            {
                var name = ParentsPath.Aggregate("", (c, parent) => c + (parent + "."));
                name += Name;

                return name;
            }
        }
        Asmdef Assembly { get; set; }

        readonly List<PackageLayout> dependencies = new List<PackageLayout>();
        public IEnumerable<PackageLayout> Dependencies => dependencies;
        public void AddDependency(PackageLayout dependency) => dependencies.Add(dependency);

        IEnumerable<PackageLayout> ParentsPath
        {
            get
            {
                var reversedParents = new List<PackageLayout>();

                var currentParent = Parent;
                while(currentParent != null)
                {
                    reversedParents.Add(Parent);
                    currentParent = currentParent.Parent;
                }

                reversedParents.Reverse();
                return reversedParents;
            }
        }
        PackageLayout Parent { get; set; }
        readonly List<PackageLayout> children = new List<PackageLayout>();
        IEnumerable<PackageLayout> Children => children;

        #region Formatting members
        public override string ToString()
        {
            var builder = new StringBuilder();
            
            builder.Append(Name);
            
            foreach(var indentedChild in Children.Select(l => IndentChild(l.ToString())))
                builder.Append("\t" + indentedChild);

            if(Assembly != null)
                builder.Append($"\n\t{Assembly.Name}.asmdef");
            
            return builder.ToString();
        }

        static string IndentChild(string child)
        {
            var childLines = child.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            var indentedChild = childLines.Aggregate("", (r, line) => $"{r}\n\t{line}");
            return indentedChild;
        }
        #endregion
       
    }
}