using System;
using System.Collections.Generic;
using System.Linq;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Constructor
{
    public interface ITypeModel : ITypeDefinition
    {
        IEnumerable<Type> DependentTypes
            => Properties
                .Select(property => property.Type)
                .Concat(Methods.SelectMany(method => method.Dependencies))
                .Distinct();
    }
}
