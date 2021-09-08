using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalendra.Commons.Tests.Editor
{
    public class AssemblyInfoFile
    {
        AssemblyBasicInfoAttributes BasicInfo;
        public IEnumerable<string> InternalsVisibleTo { get; }

        #region Constructors
        public AssemblyInfoFile(string company, string product, string title, string version, string copyright, IEnumerable<string> internalsVisibleTo)
            : this(new AssemblyBasicInfoAttributes
            {
                company = company,
                product = product,
                title = title,
                version = version,
                copyright = copyright
            }, internalsVisibleTo) { }
        
        internal AssemblyInfoFile(AssemblyBasicInfoAttributes basicInfo, IEnumerable<string> internalsVisibleTo)
        {
            BasicInfo = basicInfo;
            InternalsVisibleTo = internalsVisibleTo;
        }
        #endregion
        
        #region Properties
        public IEnumerable<string> Usings => GetUsings();

        public string Company
        {
            get => BasicInfo.company;
            set => BasicInfo.company = value;
        }
        public string Product
        {
            get => BasicInfo.product;
            set => BasicInfo.product = value;
        }
        public string Title
        {
            get => BasicInfo.title;
            set => BasicInfo.title = value;
        }
        public string Version
        {
            get => BasicInfo.version;
            set => BasicInfo.version = value;
        }
        public string Copyright
        {
            get => BasicInfo.copyright;
            set => BasicInfo.copyright = value;
        }
        #endregion

        #region Support methods
        IEnumerable<string> GetUsings()
        {
            var usings = new List<string>();

            if(!BasicInfo.Equals(default(AssemblyBasicInfoAttributes)))
                usings.Add("System.Reflection");
            
            if(InternalsVisibleTo.Any())
                usings.Add("System.Runtime.CompilerServices");

            return usings;
        }
        #endregion

        #region Conversions
        public static implicit operator string(AssemblyInfoFile assemblyInfo)
        {
            var builder = new StringBuilder();

            foreach(var reference in assemblyInfo.Usings)
                builder.AppendLine(BuildUsing(reference));
            builder.AppendLine();

            builder.AppendLine(assemblyInfo.BasicInfo);
            builder.AppendLine();

            foreach(var friend in assemblyInfo.InternalsVisibleTo)
                builder.AppendLine(BuildInternalVisibleTo(friend));
            
            return builder.ToString();
            static string BuildUsing(string reference) => $"using {reference};";
            static string BuildInternalVisibleTo(string friend) => $"[assembly: InternalsVisibleTo(\"{friend}\")]";
        }
        #endregion
    }
}