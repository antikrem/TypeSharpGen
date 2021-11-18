using System;
using System.Collections.Generic;
using System.Reflection;


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

        IEnumerable<PropertyInfo> Properties { get; }

        string OutputLocation { get; }
    }
}
