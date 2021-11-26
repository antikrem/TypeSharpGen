using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Constructor
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

        public IEnumerable<IPropertyDefinition> Properties => _innerTypeModel.Properties;

        public IEnumerable<IMethodDefinition> Methods => _innerTypeModel.Methods;

        public string OutputLocation => _innerTypeModel.OutputLocation;

    }
}
