using System.Collections.Generic;


namespace TypeSharpGenLauncher.Core.Resolution
{
    public class PropertyModel : IEmmitableSymbol
    {
        private readonly string _name;
        public IEmmitableSymbol Type { get; }

        public string Symbol => $"{_name}: {Type.Symbol}";

        public IEnumerable<ITypeModel> Dependencies => Type.Dependencies;

        public PropertyModel(IEmmitableSymbol type, string name)
        {
            Type = type;
            _name = name;
        }
    }

}
