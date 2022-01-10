using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Injection;

using TypeSharpGen.Builder;
using TypeSharpGen.Specification;
using TypeSharpGenLauncher.Core.Constructor;
using TypeSharpGenLauncher.Core.Resolution;
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
        private readonly IGenerationSpecificationFinder _generationSpecificationFinder;
        private readonly ITypeModelConstructor _dependentDefinitionMaterialisation;
        private readonly IModelResolver _modelResolver;
        private readonly ISynthesiser _synthesiser;

        public Generation(
            ITypesLoader typesLoader,
            IGenerationSpecificationFinder generationSpecificationFinder,
            ITypeModelConstructor dependentDefinitionMaterialisation,
            IModelResolver modelResolver,
            ISynthesiser synthesiser)
        {
            _typesLoader = typesLoader;
            _dependentDefinitionMaterialisation = dependentDefinitionMaterialisation;
            _modelResolver = modelResolver;
            _synthesiser = synthesiser;
            _generationSpecificationFinder = generationSpecificationFinder;
        }

        public void Generate()
        {
            var definitions = GetAllDefinitions();

            var refinement = _modelResolver.ResolveModels(definitions);

            _synthesiser.SynthesisAndWriteTypes(refinement);
        }

        private IEnumerable<ITypeDefinition> GetAllDefinitions()
        {
            var types = _typesLoader.AllTypes;

            var declarations = _generationSpecificationFinder
                .FilterSpecifications(types)
                .SelectMany(GetTypeDefinitions);

            return _dependentDefinitionMaterialisation.MaterialiseWithDependencies(declarations);
        }

        private IEnumerable<ITypeDefinition> GetTypeDefinitions(GenerationSpecification spec) 
            => spec
                .TypeDeclaractions()
                .Select(declaration => new ShiftedTypeDefinition(declaration, spec.OutputRoot));
    }
}
