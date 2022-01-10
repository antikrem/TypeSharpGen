using EphemeralEx.Extensions;
using EphemeralEx.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeSharpGen.Specification;

namespace TypeSharpGenLauncher.Loading
{
    [Injectable]
    public interface IGenerationSpecificationFinder
    {
        IEnumerable<GenerationSpecification> FilterSpecifications(IEnumerable<Type> types);
    }

    public class GenerationSpecificationFinder : IGenerationSpecificationFinder
    {
        public IEnumerable<GenerationSpecification> FilterSpecifications(IEnumerable<Type> types)
            => types
                .Where(type => type.Inherits<GenerationSpecification>())
                .Where(type => typeof(GenerationSpecification) != type)
                .Select(type => (GenerationSpecification)Activator.CreateInstance(type)!); // TODO: throw on failure
    }
}
