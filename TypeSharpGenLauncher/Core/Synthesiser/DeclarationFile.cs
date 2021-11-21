using System;
using System.Collections.Generic;
using System.Linq;
using TypeSharpGenLauncher.Core.Constructor;

namespace TypeSharpGenLauncher.Core.Synthesiser
{
    public record DeclarationFile(
        string Location,
        IEnumerable<ITypeModel> Types
    )
    {
        public IEnumerable<Type> DependentTypes 
            => Types
                .SelectMany(type => type.DependentTypes)
                .Where(type => !Types.Select(type => type.Type).Contains(type))
                .Distinct();
    }
}
