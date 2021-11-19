using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using EphemeralEx.Extensions;


namespace TypeSharpGen.Builder
{
    public record TypeBuilder(Type Type, Symbol Symbol, string Location) : ITypeDefinition
    {
        string? _name;
        public string Name => _name ?? Type.Name;
        public TypeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        HashSet<IPropertyDefinition> _declaredProperties = new();

        public TypeBuilder AddProperty(PropertyInfo propertyInfo)
        {
            if (AllProperties.None(property => property.Name == propertyInfo.Name))
                throw new Exception(); //TODO
            InnerAddProperty(propertyInfo);
            return this;
        }

        public TypeBuilder AddProperty(string propertyName)
        {
            InnerAddProperty(
                    AllProperties.Single(property => property.Name == propertyName)
                );
            return this;
        }

        private void InnerAddProperty(PropertyInfo propertyInfo)
            => _declaredProperties.Add(new PropertyDefinition(propertyInfo));

        private IEnumerable<PropertyInfo> AllProperties
            => Type.GetProperties();

        IEnumerable<IPropertyDefinition> ITypeDefinition.Properties
            => _declaredProperties;

        private string _outputLocation = Location;
        public string OutputLocation => _outputLocation;


        public ITypeDefinition EmitTo(string location)
        {
            _outputLocation = location;
            return this;
        }
    }
}
