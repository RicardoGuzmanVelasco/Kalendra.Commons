using System.Collections.Generic;

namespace Kalendra.Commons.Editor.PackageLayoutCreation.Builders
{
    public class AssemblyInfoBuilder
    {
        AssemblyBasicInfoAttributes basicInfo;
        IEnumerable<string> internalsVisibleTo = new List<string>();
        
        #region Fluent API
        public AssemblyInfoBuilder WithCompany(string company)
        {
            basicInfo.company = company;
            return this;
        }

        public AssemblyInfoBuilder WithProduct(string product)
        {
            basicInfo.product = product;
            return this;
        }

        public AssemblyInfoBuilder WithTitle(string title)
        {
            basicInfo.title = title;
            return this;
        }

        public AssemblyInfoBuilder WithVersion(string version)
        {
            basicInfo.version = version;
            return this;
        }

        public AssemblyInfoBuilder WithCopyright(string copyright)
        {
            basicInfo.copyright = copyright;
            return this;
        }

        public AssemblyInfoBuilder WithInternalsVisibleTo(params string[] references)
        {
            internalsVisibleTo = references;
            return this;
        }
        #endregion

        #region ObjectMother/FactoryMethods
        AssemblyInfoBuilder() { }

        public static AssemblyInfoBuilder New() => new AssemblyInfoBuilder();
        #endregion

        #region Builder implementation
        public AssemblyInfoFile Build() => new AssemblyInfoFile(basicInfo, internalsVisibleTo);
        public static implicit operator AssemblyInfoFile(AssemblyInfoBuilder builder) => builder.Build();
        #endregion
    }
}