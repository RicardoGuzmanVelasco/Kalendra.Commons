namespace Kalendra.Commons.Editor.PackageLayoutCreation.Builders
{
    public static class Build
    {
        public static AsmdefBuilder Asmdef() => AsmdefBuilder.New();
        public static AssemblyInfoBuilder AssemblyInfo() => AssemblyInfoBuilder.New();
    }
}