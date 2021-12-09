using System;
using System.Collections.Generic;
using System.Linq;
using TypeSharpGen.Builder;

namespace TypeSharpGenLauncher.Core.Constructor
{
    public static class TypeDefinitionExtensions
    {
        public static IEnumerable<Type> DependentTypes(this ITypeDefinition definition)
            => definition.Properties
                .Select(property => property.Type)
                .Concat(definition.Methods.SelectMany(method => method.Dependencies))
                .Distinct();
    }
}
