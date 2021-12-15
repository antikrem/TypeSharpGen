using System.Collections.Generic;


namespace TypeSharpGenLauncher.Core.Resolution
{

    public class TaskTypeModel : IEmmitableSymbol
    {
        private readonly IEmmitableSymbol _type;

        public string Symbol => $"Promise<{_type.Symbol}>";

        public IEnumerable<ITypeModel> Dependencies => _type.Dependencies;

        public TaskTypeModel(IEmmitableSymbol type)
        {
            _type = type;
        }
    }
}
