using System.Collections.Generic;


namespace TypeSharpGenLauncher.Core.Resolution
{
    public interface IEmmitableSymbol
    {
        string Symbol { get; }

        IEnumerable<ITypeModel> Dependencies { get; }
    }

}
