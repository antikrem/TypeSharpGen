using System;
using System.Collections.Generic;


namespace TypeSharpGen.Builder
{
    public interface ITypeDefinition
    {
        Type Type { get; }

        string Name { get; }

        IEnumerable<IPropertyDefinition> Properties { get; }

        IEnumerable<IMethodDefinition> Methods { get; }

        string OutputLocation { get; }
    }
}
