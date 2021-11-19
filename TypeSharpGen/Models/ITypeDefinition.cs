using System;
using System.Collections.Generic;
using System.Reflection;


namespace TypeSharpGen.Builder
{
    public enum Symbol
    {
        Class,
        Interface
    }

    public static class SymbolExtensions
    {
        static public string ToText(this Symbol symbol) 
            => Enum.GetName(symbol)!.ToLower();
    }

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

    public interface ITypeDefinition
    {
        Symbol Symbol { get; }

        Type Type { get; }

        string Name { get; }

        IEnumerable<IPropertyDefinition> Properties { get; }

        string OutputLocation { get; }
    }
}
