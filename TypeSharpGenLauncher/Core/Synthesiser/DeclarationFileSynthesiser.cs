using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EphemeralEx.Extensions;
using EphemeralEx.Injection;
using TypeSharpGen.Builder;
using TypeSharpGenLauncher.Configuration;
using TypeSharpGenLauncher.Core.Constructor;


namespace TypeSharpGenLauncher.Core.Synthesiser
{
    [Injectable]
    public interface IDeclarationFileSynthesiser
    {
        void Synthesise(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeModel> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup);
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
        )
        {
            _projectFolders = projectFolders;
            _emmisionEndpoint = emmisionEndpoint;
            _typeScriptBuiltInTypes = typeScriptBuiltInTypes;
        }

        public void Synthesise(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeModel> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
        {
            var text = InnerSynthesise(declarationFile, typeModelLookUp, declarationFileLookup);
            _emmisionEndpoint.Write($"{_projectFolders.ProjectRoot.Path}\\{declarationFile.Location}", text);
        }

        private string InnerSynthesise(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeModel> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
            => string.Join(
                    Environment.NewLine,
                    SynthesiseParts(declarationFile, typeModelLookUp, declarationFileLookup).SelectMany(sequence => sequence) // Flatten
                );

        private IEnumerable<IEnumerable<string>> SynthesiseParts(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeModel> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
        {
            yield return SynthesiseHeaderParts(declarationFile, typeModelLookUp, declarationFileLookup);
            foreach (var type in declarationFile.Types)
                yield return SynthesiseClassParts(type, typeModelLookUp);
        }

        private IEnumerable<string> SynthesiseHeaderParts(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeModel> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
        {
            yield return "// This is an auto generated test";

            var groupedImports = declarationFile.DependentTypes
                .Select(type => typeModelLookUp.ContainsKey(type) ? typeModelLookUp[type] : null)
                .NotNull()
                .GroupBy(type => declarationFileLookup[type.Type]);

            foreach (var importLine in groupedImports)
                yield return SynthesiseImport(declarationFile, importLine.Key, importLine);

            yield return string.Empty;
        }

        private string SynthesiseImport(DeclarationFile target, DeclarationFile importSource, IEnumerable<ITypeModel> typeModels)
            => $"import {{ { string.Join(", ", typeModels.Select(type => type.Name)) } }} from \"{SynthesiseImportPath(target, importSource)}\";";

        private string SynthesiseImportPath(DeclarationFile target, DeclarationFile importSource)
            => Path.GetRelativePath(target.Location, importSource.Location)
            .Substring(1)
            .Replace("\\", "/");

        private IEnumerable<string> SynthesiseClassParts(ITypeModel typeModel, IReadOnlyDictionary<Type, ITypeModel> typeModelLookUp)
        {
            yield return $"export {typeModel.Symbol.ToText()} {typeModel.Name} {{";
            foreach (var property in typeModel.Properties)
            {
                yield return $"    {property.Name}: {SynthesisePropertyType(property.Type, typeModelLookUp)};";
            }
            yield return "}";
            yield return string.Empty;
        }

        private string SynthesisePropertyType(Type type, IReadOnlyDictionary<Type, ITypeModel> typeModelLookUp)
            => _typeScriptBuiltInTypes.BuiltInTypeSymbols.TryGetValue(type, out string? value)
                ? value
                : typeModelLookUp[type].Name;
    }
}
