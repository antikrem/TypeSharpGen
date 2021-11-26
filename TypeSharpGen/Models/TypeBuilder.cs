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

        HashSet<IPropertyDefinition> _declaredProperties = new();
        public IEnumerable<IPropertyDefinition> Properties => _declaredProperties;

        HashSet<IMethodDefinition> _declaredMethods = new();
        public IEnumerable<IMethodDefinition> Methods => _declaredMethods;

        private string _outputLocation = Location;
        public string OutputLocation => _outputLocation;

        public TypeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ITypeDefinition EmitTo(string location)
        {
            _outputLocation = location;
            return this;
        }

        public TypeBuilder AddProperty(PropertyInfo propertyInfo)
        {
            if (AllProperties.None(property => property.Name == propertyInfo.Name))
                throw new Exception(); //TODO
            InnerAddProperty(propertyInfo);
            return this;
        }

        public TypeBuilder AddProperty(string propertyName)
        {
            InnerAddProperty(AllProperties.Single(property => property.Name == propertyName));
            return this;
        }

        private void InnerAddProperty(PropertyInfo propertyInfo)
            => _declaredProperties.Add(new PropertyDefinition(propertyInfo));

        public TypeBuilder AddMethod(MethodInfo methodInfo)
        {
            if (AllMethods.None(property => property.Name == methodInfo.Name))
                throw new Exception(); //TODO
            AddMethod(methodInfo);
            return this;
        }

        public TypeBuilder AddMethod(string methodName)
        {
            InnerAddMethod(AllMethods.Single(method => method.Name == methodName));
            return this;
        }

        private void InnerAddMethod(MethodInfo methodInfo)
            => _declaredMethods.Add(new MethodDefinition(methodInfo));

        private IEnumerable<PropertyInfo> AllProperties
            => Type.GetProperties();

        private IEnumerable<MethodInfo> AllMethods
            => Type.GetMethods();
    }
}
