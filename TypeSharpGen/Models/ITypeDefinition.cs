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
        Type Type { get; }

        string Name { get; }

        IEnumerable<IPropertyDefinition> Properties { get; }

        IEnumerable<IMethodDefinition> Methods { get; }

        string OutputLocation { get; }
    }
}
