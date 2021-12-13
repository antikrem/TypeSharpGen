using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Constructor
{
    public class DefaultTypeDefinition : ITypeDefinition
    {
        private readonly Type _type;
        private readonly ITypeDefinition _parent;

        public DefaultTypeDefinition(Type type, ITypeDefinition parent)
        {
            _type = type;
            _parent = parent;
        }

        public Type Type => _type;
        public string Name => _type.Name;

        public IEnumerable<IPropertyDefinition> Properties => _type.GetProperties().Select(property => new PropertyDefinition(property));
        
        public IEnumerable<IMethodDefinition> Methods => Enumerable.Empty<IMethodDefinition>();

        public string OutputLocation => _parent.OutputLocation;
    }
}
