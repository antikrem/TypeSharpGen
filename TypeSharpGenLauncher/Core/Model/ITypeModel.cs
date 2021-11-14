using System;
using System.Collections.Generic;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Model
{
    public interface ITypeModel : ITypeDefinition
    {
        IEnumerable<Type> DependentTypes { get; }
    }
}
