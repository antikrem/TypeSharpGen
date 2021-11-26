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
        private readonly ITypeModelConstructor _typeModelBuilder;
        private readonly ISynthesiser _synthesiser;

        public Generation(ITypesLoader typesLoader, ITypeModelConstructor typeModelBuilder, IDeclarationFileSynthesiser declarationFileSynthesiser, ISynthesiser synthesiser)
        {
            _typesLoader = typesLoader;
            _typeModelBuilder = typeModelBuilder;
            _synthesiser = synthesiser;
        }

        public void Generate()
        {
            var types = _typesLoader.AllTypes();

            var declarations = types
                .Where(type => type.Inherits<GenerationSpecification>())
                .Where(type => typeof(GenerationSpecification) != type)
                .SelectMany(GetTypeDefinitions);

            var models = _typeModelBuilder.ConstructTypedModels(declarations);

            _synthesiser.SynthesisAndWriteTypes(models);
        }

        private IEnumerable<ITypeDefinition> GetTypeDefinitions(Type specType)
        {
            var spec = (GenerationSpecification)Activator.CreateInstance(specType);
            return spec.TypeDeclaractions();
        }
    }
}
