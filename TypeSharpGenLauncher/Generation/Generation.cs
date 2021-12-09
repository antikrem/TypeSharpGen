using System;
using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Extensions;
using EphemeralEx.Injection;

using TypeSharpGen.Builder;
using TypeSharpGen.Specification;
using TypeSharpGenLauncher.Core.Constructor;
using TypeSharpGenLauncher.Core.Synthesiser;
using TypeSharpGenLauncher.Loading;


namespace TypeSharpGenLauncher.Generation
{
    [Injectable]
    public interface IGeneration
    {
        void Generate();
    }

    public class Generation : IGeneration
    {
        private readonly ITypesLoader _typesLoader;
        private readonly IDependentDefinitionMaterialisation _dependentDefinitionMaterialisation;
        private readonly ISynthesiser _synthesiser;

        public Generation(ITypesLoader typesLoader, IDependentDefinitionMaterialisation dependentDefinitionMaterialisation, ISynthesiser synthesiser)
        {
            _typesLoader = typesLoader;
            _dependentDefinitionMaterialisation = dependentDefinitionMaterialisation;
            _synthesiser = synthesiser;
        }

        public void Generate()
        {
            var definitions = GetAllDefinitions();

            _synthesiser.SynthesisAndWriteTypes(definitions);
        }

        private IEnumerable<ITypeDefinition> GetAllDefinitions()
        {
            var types = _typesLoader.AllTypes();

            var declarations = types
                .Where(type => type.Inherits<GenerationSpecification>())
                .Where(type => typeof(GenerationSpecification) != type)
                .SelectMany(GetTypeDefinitions);

            return _dependentDefinitionMaterialisation.MaterialiseWithDependencies(declarations);
        }

        private IEnumerable<ITypeDefinition> GetTypeDefinitions(Type specType)
        {
            var spec = (GenerationSpecification)Activator.CreateInstance(specType);
            return spec
                .TypeDeclaractions()
                .Select(declaration => new ShiftedTypeDefinition(declaration, spec.OutputRoot));
        }
    }
}
