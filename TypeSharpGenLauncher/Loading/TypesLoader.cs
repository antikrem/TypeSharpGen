using EphemeralEx.Extensions;
using EphemeralEx.Injection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeSharpGenLauncher.Configuration;

namespace TypeSharpGenLauncher.Loading
{
    [Injectable]
    public interface ITypesLoader
    {
        IEnumerable<Type> AllTypes { get; }
    }

    public class TypesLoader : ITypesLoader
    {
        private readonly IProjectFolders _projectFolders;
        private readonly IAssemblyLoader _assemblyLoader;

        public TypesLoader(IProjectFolders projectFolders, IAssemblyLoader assemblyLoader)
        {
            _projectFolders = projectFolders;
            _assemblyLoader = assemblyLoader;
        }

        public IEnumerable<Type> AllTypes
            => _projectFolders
                .BinaryDirectories
                .First()
                .TraverseTree(directory => directory.ChildDirectories)
                .SelectMany(directory => directory.ChildFiles)
                .Where(file => file.Extension == ".dll") //TODO: pull file type check to extension in EphemeralEx
                .Select(TryLoadAssemblyTypes)
                .Flatten();

        private IEnumerable<Type> TryLoadAssemblyTypes(EphemeralEx.FileSystem.File file)
        {
            using var context = _assemblyLoader.Context();
            try
            {
                return Assembly.LoadFrom(file.Path).GetTypes();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Type>();
            }
        }
    }
}
