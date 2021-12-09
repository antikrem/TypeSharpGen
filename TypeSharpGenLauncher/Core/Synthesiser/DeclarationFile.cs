using System;
using System.Collections.Generic;
using System.Linq;
using TypeSharpGen.Builder;
using TypeSharpGenLauncher.Core.Constructor;

namespace TypeSharpGenLauncher.Core.Synthesiser
{
    public record DeclarationFile(
        string Location,
        IEnumerable<ITypeDefinition> Types
    )
    {
        public IEnumerable<Type> CalculateDependentTypes 
            => Types
                .SelectMany(type => type.DependentTypes())
                .Where(type => !Types.Select(type => type.Type).Contains(type))
                .Distinct();
    }
}
