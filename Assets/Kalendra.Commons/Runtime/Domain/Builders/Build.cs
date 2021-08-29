namespace Kalendra.Commons.Runtime.Domain.Builders
{
    public static class Build
    {
        public static JsonPairBuilder<T> JsonPair<T>() => JsonPairBuilder<T>.New();
    }
}