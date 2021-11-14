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

    public interface ITypeDefinition
    {
        Symbol Symbol { get; }

        Type Type { get; }

        string Name { get; }

        IEnumerable<PropertyInfo> Properties { get; }

        string OutputLocation { get; }
    }
}
