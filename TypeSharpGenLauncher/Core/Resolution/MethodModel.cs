using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Extensions;


namespace TypeSharpGenLauncher.Core.Resolution
{
    public class ParameterModel : IEmmitableSymbol
    {
        private readonly string _name;
        public IEmmitableSymbol Type { get; }

        public string Symbol => $"{_name}: {Type.Symbol}";

        public IEnumerable<ITypeModel> Dependencies => Type.Dependencies;

        public ParameterModel(IEmmitableSymbol type, string name)
        {
            Type = type;
            _name = name;
        }
    }

    public class MethodModel : IEmmitableSymbol
    {
        private readonly string _name;

        public readonly IEmmitableSymbol _returnType;
        public readonly IEnumerable<ParameterModel> _parameters;

        public string Symbol => $"{_name}({string.Join(" ,", _parameters.Select(paramter => paramter.Symbol))}): {_returnType.Symbol}";

        public IEnumerable<ITypeModel> Dependencies 
            => Sequence.From(
                _returnType.Dependencies,
                _parameters.SelectMany(parameter => parameter.Dependencies)
            );

        public MethodModel(IEmmitableSymbol returnType, IEnumerable<ParameterModel> parameters, string name)
        {
            _returnType = returnType;
            _parameters = parameters;
            _name = name;
        }
    }

}
