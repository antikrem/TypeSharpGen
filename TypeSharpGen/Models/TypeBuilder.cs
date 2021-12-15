using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using EphemeralEx.Extensions;


namespace TypeSharpGen.Builder
{
    public interface IMethodModifier
    {
        MethodDefinition Apply(MethodDefinition method);
    }

    public class OverideReturnType : IMethodModifier
    {
        private Type _type;

        public OverideReturnType(Type type)
        {
            _type = type;
        }

        public MethodDefinition Apply(MethodDefinition method)
        {
            method.OverrideReturnType(_type);
            return method;
        }
    }

    public class OverideReturnType<T> : IMethodModifier
    {
        public MethodDefinition Apply(MethodDefinition method)
        {
            method.OverrideReturnType(typeof(T));
            return method;
        }
    }

    public record TypeBuilder(Type Type, string Location) : ITypeDefinition
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

        public TypeBuilder AddMethod(string methodName, params IMethodModifier[] modifiers)
        {
            InnerAddMethod(AllMethods.Single(method => method.Name == methodName), modifiers);
            return this;
        }

        private void InnerAddMethod(MethodInfo methodInfo, IMethodModifier[] modifiers)
            => _declaredMethods
                .Add(
                    new MethodDefinition(methodInfo)
                        .ChainCall(modifiers, (definition, modifier) => modifier.Apply(definition))
                );


        private IEnumerable<PropertyInfo> AllProperties
            => Type.GetProperties();

        private IEnumerable<MethodInfo> AllMethods
            => Type.GetMethods();
    }
}
