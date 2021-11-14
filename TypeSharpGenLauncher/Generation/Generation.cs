using EphemeralEx.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeSharpGen.Builder;
using TypeSharpGen.Specification;
using TypeSharpGenLauncher.Core.Builder;
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
        private readonly ITypeModelBuilder _typeModelBuilder;
        private readonly IDeclarationFileSynthesiser _declarationFileSynthesiser;

        public Generation(ITypesLoader typesLoader, ITypeModelBuilder typeModelBuilder, IDeclarationFileSynthesiser declarationFileSynthesiser)
        {
            _typesLoader = typesLoader;
            _typeModelBuilder = typeModelBuilder;
            _declarationFileSynthesiser = declarationFileSynthesiser;
        }

        public void Generate()
        {
            var types = _typesLoader.AllTypes();

            var declarations = types
                .Where(type => typeof(GenerationSpecification).IsAssignableFrom(type))
                .Where(type => typeof(GenerationSpecification) != type)
                .SelectMany(GetTypeDefinitions);

            var models = _typeModelBuilder.ResolveTypedModels(declarations);

            var declarationFiles = models.GroupBy(model => model.OutputLocation)
                .Select(group => new DeclarationFile(group.Key, group));

            _declarationFileSynthesiser.Synthesise(declarationFiles);
        }

        private IEnumerable<ITypeDefinition> GetTypeDefinitions(Type specType)
        {
            var spec = (GenerationSpecification)Activator.CreateInstance(specType);
            return spec.TypeDeclaractions();
        }

    }
}
