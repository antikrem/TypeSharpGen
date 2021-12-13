using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Constructor
{
    public class ShiftedTypeDefinition : ITypeDefinition
    {
        private readonly ITypeDefinition _inner;
        private readonly string _root;

        public ShiftedTypeDefinition(ITypeDefinition inner, string root)
        {
            _inner = inner;
            _root = root;
        }

        public Type Type => _inner.Type;

        public string Name => _inner.Name;

        public IEnumerable<IPropertyDefinition> Properties => _inner.Properties;

        public IEnumerable<IMethodDefinition> Methods => _inner.Methods;

        public string OutputLocation => Path.Join(_root, _inner.OutputLocation);
    }
}
