using System;
using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Injection;
using TypeSharpGen.Builder;
using TypeSharpGenLauncher.Configuration;
using TypeSharpGenLauncher.Core.Constructor;


namespace TypeSharpGenLauncher.Core.Synthesiser
{
    [Injectable]
    public interface IDeclarationFileSynthesiser
    {
        void Synthesise(DeclarationFile declarationFile, IDictionary<Type, ITypeModel> typeModelLookUp);
    }

    public class DeclarationFileSynthesiser : IDeclarationFileSynthesiser
    {
        private readonly IProjectFolders _projectFolders;
        private readonly IEmisionEndpoint _emmisionEndpoint;
        private readonly ITypeScriptBuiltInTypes _typeScriptBuiltInTypes;

        public DeclarationFileSynthesiser(
            IProjectFolders projectFolders,
            IEmisionEndpoint emmisionEndpoint,
            ITypeScriptBuiltInTypes typeScriptBuiltInTypes
        ) {
            _projectFolders = projectFolders;
            _emmisionEndpoint = emmisionEndpoint;
            _typeScriptBuiltInTypes = typeScriptBuiltInTypes;
        }

        public void Synthesise(DeclarationFile declarationFile, IDictionary<Type, ITypeModel> typeModelLookUp)
        {
            var text = InnerSynthesise(declarationFile, typeModelLookUp);
            _emmisionEndpoint.Write($"{_projectFolders.ProjectRoot.Path}\\{declarationFile.Location}", text);
        }

        private string InnerSynthesise(DeclarationFile declarationFile, IDictionary<Type, ITypeModel> typeModelLookUp)
            => string.Join(
                    Environment.NewLine, 
                    SynthesiseParts(declarationFile, typeModelLookUp).SelectMany(sequence => sequence) // Flatten
                );

        private IEnumerable<IEnumerable<string>> SynthesiseParts(DeclarationFile declarationFile, IDictionary<Type, ITypeModel> typeModelLookUp)
        {
            yield return SynthesiseHeaderParts();
            foreach (var type in declarationFile.Types)
                yield return SynthesiseClassParts(type, typeModelLookUp);
        }

        private IEnumerable<string> SynthesiseHeaderParts()
        {
            yield return "// This is an auto generated test";
            yield return string.Empty;
        }

        private IEnumerable<string> SynthesiseClassParts(ITypeModel typeModel, IDictionary<Type, ITypeModel> typeModelLookUp)
        {
            yield return $"export {typeModel.Symbol.ToText()} {typeModel.Name} {{";
            foreach (var property in typeModel.Properties)
            {
                yield return $"    {property.Name}: {SynthesisePropertyType(property.Type, typeModelLookUp)};";
            }
            yield return "}";
            yield return string.Empty;
        }

        private string SynthesisePropertyType(Type type, IDictionary<Type, ITypeModel> typeModelLookUp)
            => _typeScriptBuiltInTypes.BuiltInTypeSymbols.TryGetValue(type, out string? value)
                ? value
                : typeModelLookUp[type].Name;
    }
}
