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

        private string? _overrideName = null;
        public string Name => _overrideName ?? _propertyInfo.Name;

        public PropertyDefinition(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void OverrideName(string name)
        {
            _overrideName = name;
        }
    }
}
