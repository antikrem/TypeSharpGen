using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Constructor
{
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
        public string Name => _type.Name;

        public IEnumerable<IPropertyDefinition> Properties => _type.GetProperties().Select(property => new PropertyDefinition(property));
        
        public IEnumerable<IMethodDefinition> Methods => Enumerable.Empty<IMethodDefinition>();

        public string OutputLocation => _parent.OutputLocation;
    }
}
