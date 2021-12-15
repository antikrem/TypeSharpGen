using System.Collections.Generic;
using System.Linq;


namespace TypeSharpGenLauncher.Core.Resolution
{
    public class DefaultImplementation : IEmmitableSymbol
    {
        public string Symbol { get; }

        public IEnumerable<ITypeModel> Dependencies => Enumerable.Empty<ITypeModel>();

        public DefaultImplementation(string name)
        {
            Symbol = name;
        }
    }

}
