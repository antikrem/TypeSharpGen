using System;
using System.Reflection;


namespace TypeSharpGen.Builder
{
    public interface IPropertyDefinition
    {
        Type Type { get; }

        string Name { get; }
    }

    public class PropertyDefinition : IPropertyDefinition
    {
        private readonly PropertyInfo _propertyInfo;

        public Type Type => _propertyInfo.PropertyType;

        public string Name => _propertyInfo.Name;

        public PropertyDefinition(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }
    }
}
