namespace Kalendra.Commons.Runtime.Domain.Builders
{
    public class JsonPairBuilder<T>
    {
        public string Name { get; private set; }
        public T Value { get; private set; }

        #region Fluent API
        public JsonPairBuilder<T> WithName(string name)
        {
            Name = name;
            return this;
        }

        public JsonPairBuilder<T> WithValue(T value)
        {
            Value = value;
            return this;
        }
        #endregion

        #region ObjectMother/FactoryMethods
        JsonPairBuilder() { }

        public static JsonPairBuilder<T> New() => new JsonPairBuilder<T>();
        #endregion

        #region Builder implementation
        public string Build() => $"\"{Name}\": {Print(Value)}";
        public static implicit operator string(JsonPairBuilder<T> builder) => builder.Build();
        #endregion
        
        #region Support methods
        static string Print(T value)
        {
            return value is bool
                ? value.ToString().ToLower()
                : $"\"{value}\"";
        }
        #endregion
    }
}