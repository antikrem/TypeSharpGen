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
        void Synthesise(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup);
    }

    public class DeclarationFileSynthesiser : IDeclarationFileSynthesiser
    {
        private readonly IProjectFolders _projectFolders;
        private readonly IEnumerable<IEmisionEndpoint> _emmisionEndpoints;
        private readonly ITypeScriptBuiltInTypes _typeScriptBuiltInTypes;
        private readonly ITypeReducer _typeReducer;

        public DeclarationFileSynthesiser(
            IProjectFolders projectFolders,
            IEnumerable<IEmisionEndpoint> emmisionEndpoints,
            ITypeScriptBuiltInTypes typeScriptBuiltInTypes,
            ITypeReducer typeReducer
        )
        {
            _projectFolders = projectFolders;
            _emmisionEndpoints = emmisionEndpoints;
            _typeScriptBuiltInTypes = typeScriptBuiltInTypes;
            _typeReducer = typeReducer;
        }

        public void Synthesise(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
        {
            var text = InnerSynthesise(declarationFile, typeModelLookUp, declarationFileLookup);
            _emmisionEndpoints.ForEach(endpoint => endpoint.Write($"{_projectFolders.ProjectRoot.Path}\\{declarationFile.Location}", text));
        }

        private string InnerSynthesise(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
            => string.Join(
                    Environment.NewLine,
                    SynthesiseParts(declarationFile, typeModelLookUp, declarationFileLookup).Flatten()
                );

        private IEnumerable<IEnumerable<string>> SynthesiseParts(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
        {
            yield return SynthesiseHeaderParts(declarationFile, typeModelLookUp, declarationFileLookup);
            foreach (var type in declarationFile.Types)
                yield return SynthesiseClassParts(type, typeModelLookUp);
        }

        private IEnumerable<string> SynthesiseHeaderParts(DeclarationFile declarationFile, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IReadOnlyDictionary<Type, DeclarationFile> declarationFileLookup)
        {
            yield return "// This is an auto generated test";

            var groupedImports = declarationFile
                .CalculateDependentTypes
                .Select(_typeReducer.Reduce)
                .Select(type => typeModelLookUp.ContainsKey(type) ? typeModelLookUp[type] : null)
                .NotNull()
                .GroupBy(type => declarationFileLookup[type.Type]);

            foreach (var importLine in groupedImports)
                if (declarationFile.Location != importLine.Key.Location)
                    yield return SynthesiseImport(declarationFile, importLine.Key, importLine);

            yield return string.Empty;
        }

        private string SynthesiseImport(DeclarationFile target, DeclarationFile importSource, IEnumerable<ITypeDefinition> typeModels)
            => $"import {{ { string.Join(", ", typeModels.Select(type => type.Name)) } }} from \"{SynthesiseImportPath(target, importSource)}\";";

        private string SynthesiseImportPath(DeclarationFile target, DeclarationFile importSource)
            => Path.GetRelativePath(target.Location, importSource.Location.Split('.').First())
                .Substring(1)
                .Replace("\\", "/");

        private IEnumerable<string> SynthesiseClassParts(ITypeDefinition typeModel, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp)
        {
            yield return $"export {typeModel.Symbol.ToText()} {typeModel.Name} {{";

            foreach (var property in typeModel.Properties)
                yield return SynthesiseProperty(typeModelLookUp, property);

            foreach (var method in typeModel.Methods)
            {
                yield return string.Empty;
                yield return SynthesiseMethod(typeModelLookUp, method);
            }

            yield return "}";
            yield return string.Empty;
        }

        private string SynthesiseProperty(IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IPropertyDefinition property)
            => $"    {property.Name}: {SynthesisePropertyType(property.Type, typeModelLookUp)};";

        private string SynthesiseMethod(IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IMethodDefinition method)
            => $"    {method.Name}({SynthesisePrarameters(typeModelLookUp, method)}): {SynthesisePropertyType(method.ReturnType, typeModelLookUp)};";

        private string SynthesisePrarameters(IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IMethodDefinition method)
            => string.Join(", ", method.Parameters.Select(parameter => SynthesisePropertyType(typeModelLookUp, parameter)));

        private string SynthesisePropertyType(IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp, IParameterDefinition parameter)
            => $"{parameter.Name}: {SynthesisePropertyType(parameter.Type, typeModelLookUp)}";

        private string SynthesisePropertyType(Type type, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp)
            => _typeScriptBuiltInTypes.BuiltInTypeSymbols.TryGetValue(type, out string? value)
                ? value
                : SynthesiseDeclaredType(type, typeModelLookUp);

        private string SynthesiseDeclaredType(Type type, IReadOnlyDictionary<Type, ITypeDefinition> typeModelLookUp)
        {
            var reducedType = _typeReducer.Reduce(type);
            var reducedName = typeModelLookUp[reducedType].Name;

            return IsIterableType(type)
                ? $"{reducedName}[]"
                : reducedName;
        }

        private bool IsIterableType(Type type) 
            => type.IsGenericType && _typeReducer.IterableTypes.Contains(type.GetGenericTypeDefinition())
                || type.IsArray;
    }
}
