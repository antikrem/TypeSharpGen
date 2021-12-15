using System.Collections.Generic;


namespace TypeSharpGenLauncher.Core.Resolution
{
    public class ArrayTypeModel : IEmmitableSymbol
    {
        private readonly IEmmitableSymbol _type;

        public string Symbol => $"{_type.Symbol}[]";

        public IEnumerable<ITypeModel> Dependencies => _type.Dependencies;

        public ArrayTypeModel(IEmmitableSymbol type)
        {
            _type = type;
        }
    }

    public class DictionaryTypeModel : IEmmitableSymbol
    {
        private readonly IEmmitableSymbol _key;
        private readonly IEmmitableSymbol _value;

        public string Symbol => $"Record<{_key.Symbol}, {_value.Symbol}>";

        public IEnumerable<ITypeModel> Dependencies => _key.Dependencies;

        public DictionaryTypeModel(IEmmitableSymbol key, IEmmitableSymbol value)
        {
            _key = key;
            _value = value;
        }
    }
}
