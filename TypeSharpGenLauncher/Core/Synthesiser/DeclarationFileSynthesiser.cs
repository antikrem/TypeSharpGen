using EphemeralEx.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeSharpGenLauncher.Configuration;
using TypeSharpGenLauncher.Core.Model;

namespace TypeSharpGenLauncher.Core.Synthesiser
{
    [Injectable]
    public interface IDeclarationFileSynthesiser
    {
        void Synthesise(IEnumerable<DeclarationFile> declarationFiles);
    }

    public class DeclarationFileSynthesiser : IDeclarationFileSynthesiser
    {
        private readonly IProjectFolders _projectFolders;
        private readonly IEmisionEndpoint _emmisionEndpoint;

        public DeclarationFileSynthesiser(IProjectFolders projectFolders, IEmisionEndpoint emmisionEndpoint)
        {
            _projectFolders = projectFolders;
            _emmisionEndpoint = emmisionEndpoint;
        }

        public void Synthesise(IEnumerable<DeclarationFile> declarationFiles)
        {
            foreach (var file in declarationFiles)
            {
                var text = Synthesise(file);
                _emmisionEndpoint.Write($"{_projectFolders.ProjectRoot.Path}\\{file.Location}", text);
            }
        }

        private string Synthesise(DeclarationFile declarationFile)
            => string.Join(Environment.NewLine, SynthesiseParts(declarationFile).SelectMany(sequence => sequence)); // Flatten

        private IEnumerable<IEnumerable<string>> SynthesiseParts(DeclarationFile declarationFile)
        {
            yield return SynthesiseHeaderParts();
            foreach (var type in declarationFile.Types)
                yield return SynthesiseClassParts(type);
        }

        private IEnumerable<string> SynthesiseHeaderParts()
        {
            yield return "// This is an auto generated test";
            yield return string.Empty;
        }

        private IEnumerable<string> SynthesiseClassParts(ITypeModel typeModel)
        {
            yield return $"export {typeModel.Symbol} {typeModel.Name} {{";
            foreach (var property in typeModel.Properties)
            {
                yield return $"    {property.Name}: {property.PropertyType.Name};";
            }
            yield return "}";
            yield return string.Empty;
        }
    }
}
