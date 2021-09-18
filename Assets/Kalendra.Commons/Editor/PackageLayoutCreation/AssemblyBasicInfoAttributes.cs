using System.Text;

namespace Kalendra.Commons.Editor.PackageLayoutCreation
{
    internal struct AssemblyBasicInfoAttributes
    {
        internal string company;
        internal string product;
        internal string title;

        internal string version;
        internal string copyright;

        public static implicit operator string(AssemblyBasicInfoAttributes source)
        {
            var builder = new StringBuilder();
            
            builder.Append(BuildAttribute("Company", source.company));
            builder.Append(BuildAttribute("Product", source.product));
            builder.Append(BuildAttribute("Title", source.title));
        
            builder.Append(BuildAttribute("Version", source.version));
            builder.Append(BuildAttribute("Copyright", source.copyright));

            return builder.ToString();
            static string BuildAttribute(string key, string value) => value is null ? "" : $"[assembly: Assembly{key}(\"{value}\")]\r\n";
        }
    }
}