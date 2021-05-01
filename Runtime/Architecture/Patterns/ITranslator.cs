using JetBrains.Annotations;

namespace Kalendra.Commons.Runtime.Architecture.Patterns
{
    public interface ITranslator<T1, T2>
    {
        T1 Translate([NotNull] T2 source);
        T2 Translate([NotNull] T1 source);
    }
}