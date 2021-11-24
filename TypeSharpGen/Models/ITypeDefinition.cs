using System;
using System.Collections.Generic;


namespace TypeSharpGen.Builder
{
    public enum Symbol
    {
        Class,
        Interface
    }

    public static class SymbolExtensions
    {
        static public string ToText(this Symbol symbol) 
            => Enum.GetName(symbol)!.ToLower();
    }

    public interface ITypeDefinition
    {
        Symbol Symbol { get; }

        Type Type { get; }

        string Name { get; }

        IEnumerable<IPropertyDefinition> Properties { get; }

        string OutputLocation { get; }
    }
}
