using System;
using System.Collections.Generic;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Constructor
{
    public interface ITypeModel : ITypeDefinition
    {
        IEnumerable<Type> DependentTypes { get; }
    }
}
