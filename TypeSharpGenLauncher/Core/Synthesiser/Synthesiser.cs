using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Extensions;
using EphemeralEx.Injection;

using TypeSharpGenLauncher.Core.Constructor;


namespace TypeSharpGenLauncher.Core.Synthesiser
{

    [Injectable]
    public interface ISynthesiser
    {
        void SynthesisAndWriteTypes(IEnumerable<ITypeModel> typeModels);
    }

    public class Synthesiser : ISynthesiser
    {
        private readonly IDeclarationFileSynthesiser _declarationFileSynthesiser;

        public Synthesiser(IDeclarationFileSynthesiser declarationFileSynthesiser)
        {
            _declarationFileSynthesiser = declarationFileSynthesiser;
        }

        public void SynthesisAndWriteTypes(IEnumerable<ITypeModel> typeModels)
        {
            var declarations = typeModels
                .DistinctBy(model => model.Type)
                .GroupBy(model => model.OutputLocation)
                .Select(group => new DeclarationFile(group.Key, group));

            // TODO: Split these into a seperate packing step to move logic out of synthesis
            var lookUp = typeModels
                .DistinctBy(model => model.Type)
                .IndexBy(model => model.Type);

            var declarationFileLookup = typeModels
                .DistinctBy(model => model.Type)
                .ToDictionary(
                    model => model.Type, 
                    model => declarations.Single(file => file.Types.Select(type => type.Type).Contains(model.Type))
                );

            foreach (var declaration in declarations)
                _declarationFileSynthesiser.Synthesise(declaration, lookUp, declarationFileLookup);
        }
    }
}
