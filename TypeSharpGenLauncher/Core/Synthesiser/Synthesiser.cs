using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Injection;

using TypeSharpGenLauncher.Core.Resolution;


namespace TypeSharpGenLauncher.Core.Synthesiser
{

    [Injectable]
    public interface ISynthesiser
    {
        void SynthesisAndWriteTypes(IEnumerable<ITypeModel> models);
    }

    public class Synthesiser : ISynthesiser
    {
        private readonly IDeclarationFileSynthesiser _declarationFileSynthesiser;

        public Synthesiser(IDeclarationFileSynthesiser declarationFileSynthesiser)
        {
            _declarationFileSynthesiser = declarationFileSynthesiser;
        }

        public void SynthesisAndWriteTypes(IEnumerable<ITypeModel> models)
        {
            var declarations = models
                .GroupBy(model => model.OutputLocation)
                .Select(group => new DeclarationFile(group.Key, group));

            foreach (var declaration in declarations)
                _declarationFileSynthesiser.Synthesise(declaration, declarations);
        }
    }
}
