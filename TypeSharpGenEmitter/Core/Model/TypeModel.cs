using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeSharpGen.Builder;

namespace TypeSharpGenEmitter.Core.Model
{
    public interface ITypeModel : ITypeDefinition
    {
        IEnumerable<Type> DependentTypes { get; }
    }

    public class TypeModel : ITypeModel
    {
        private readonly ITypeDefinition _innerTypeModel;

        public TypeModel(ITypeDefinition innerTypeModel)
        {
            _innerTypeModel = innerTypeModel;
        }

        public Symbol Symbol => _innerTypeModel.Symbol;
        public Type Type => _innerTypeModel.Type;

        public IEnumerable<PropertyInfo> Properties => _innerTypeModel.Properties;

        public string OutputLocation => _innerTypeModel.OutputLocation;

        public IEnumerable<Type> DependentTypes => Properties.Select(property => property.PropertyType);
    }

    public class DefaultTypeModel : ITypeModel
    {
        private readonly Type _type;
        private readonly ITypeDefinition _parent;

        public DefaultTypeModel(Type type, ITypeDefinition parent)
        {
            _type = type;
            _parent = parent;
        }

        public Symbol Symbol => Symbol.Interface;
        public Type Type => _type;

        public IEnumerable<PropertyInfo> Properties => _type.GetProperties();

        public string OutputLocation => _parent.OutputLocation;

        public IEnumerable<Type> DependentTypes => Properties.Select(property => property.PropertyType);
    }
}
