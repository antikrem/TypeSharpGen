﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EphemeralEx.Extensions;
using EphemeralEx.Injection;

using TypeSharpGenLauncher.Configuration;
using TypeSharpGenLauncher.Core.Resolution;


namespace TypeSharpGenLauncher.Core.Synthesiser
{
    [Injectable]
    public interface IDeclarationFileSynthesiser
    {
        void Synthesise(DeclarationFile declarationFile, IEnumerable<DeclarationFile> declarationFiles);
    }

    public class DeclarationFileSynthesiser : IDeclarationFileSynthesiser
    {
        private readonly IProjectFolders _projectFolders;
        private readonly IEnumerable<IEmisionEndpoint> _emmisionEndpoints;

        public DeclarationFileSynthesiser(IProjectFolders projectFolders, IEnumerable<IEmisionEndpoint> emmisionEndpoints)
        {
            _projectFolders = projectFolders;
            _emmisionEndpoints = emmisionEndpoints;
        }

        public void Synthesise(DeclarationFile declarationFile, IEnumerable<DeclarationFile> declarationFiles)
        {
            var text = InnerSynthesise(declarationFile, declarationFiles);
            _emmisionEndpoints.ForEach(endpoint => endpoint.Write($"{_projectFolders.ProjectRoot.Path}\\{declarationFile.Location}", text));
        }

        private string InnerSynthesise(DeclarationFile declarationFile, IEnumerable<DeclarationFile> declarationFiles)
            => string.Join(
                    Environment.NewLine,
                    SynthesiseParts(declarationFile, declarationFiles).Flatten()
                );

        private IEnumerable<IEnumerable<string>> SynthesiseParts(DeclarationFile declarationFile, IEnumerable<DeclarationFile> declarationFiles)
        {
            yield return SynthesiseHeaderParts(declarationFile, declarationFiles);
            
            foreach (var type in declarationFile.Types)
                yield return SynthesiseClassParts(type);
        }

        private IEnumerable<string> SynthesiseHeaderParts(DeclarationFile declarationFile, IEnumerable<DeclarationFile> declarationFiles)
        {
            yield return "// This is an auto generated test";

            var groupedImports = declarationFile
                .Dependencies
                .GroupBy(type => declarationFiles.Single(possibleFile => possibleFile.Types.Contains(type)), new DeclarationFileComparer())
                .Where(import => import.Key.Location != declarationFile.Location);

            foreach (var import in groupedImports)
                yield return SynthesiseImport(declarationFile, import.Key, import);

            yield return string.Empty;
        }

        private string SynthesiseImport(DeclarationFile target, DeclarationFile importSource, IEnumerable<ITypeModel> typeModels)
            => $"import {{ { string.Join(", ", typeModels.Select(type => type.Symbol)) } }} from \"{SynthesiseImportPath(target, importSource)}\";";

        private string SynthesiseImportPath(DeclarationFile target, DeclarationFile importSource)
            => Path.GetRelativePath(target.Location, importSource.Location.Split('.').First())
                .Substring(1)
                .Replace("\\", "/");

        private IEnumerable<string> SynthesiseClassParts(ITypeModel model)
        {
            yield return $"export interface {model.Symbol} {{";

            foreach (var property in model.Properties)
                yield return $"    {property.Symbol};";

            foreach (var method in model.Methods)
            {
                yield return string.Empty;
                yield return $"    {method.Symbol};";
            }

            yield return "}";
            yield return string.Empty;
        }

    }
}
