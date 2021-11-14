using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Model
{
    public class TypeModel : ITypeModel
    {
        private readonly ITypeDefinition _innerTypeModel;

        public TypeModel(ITypeDefinition innerTypeModel)
        {
            _innerTypeModel = innerTypeModel;
        }

        public Symbol Symbol => _innerTypeModel.Symbol;
        public Type Type => _innerTypeModel.Type;
        public string Name => _innerTypeModel.Name;

        public IEnumerable<PropertyInfo> Properties => _innerTypeModel.Properties;

        public string OutputLocation => _innerTypeModel.OutputLocation;

        public IEnumerable<Type> DependentTypes => Properties.Select(property => property.PropertyType);
    }
}
