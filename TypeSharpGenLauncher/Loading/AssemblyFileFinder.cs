using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using EphemeralEx.Extensions;
using EphemeralEx.FileSystem;
using EphemeralEx.Injection;

using TypeSharpGenLauncher.Configuration;


namespace TypeSharpGenLauncher.Loading
{

    [Injectable]
    public interface IAssemblyFileFinder
    {
        IEnumerable<File> FindMacthingAssemblyFiles(AssemblyName assemblyName);
    }

    public class AssemblyFileFinder : IAssemblyFileFinder
    {
        private readonly IProjectFolders _projectFolders;

        public AssemblyFileFinder(IProjectFolders projectFolders)
        {
            _projectFolders = projectFolders;
        }

        public IEnumerable<File> FindMacthingAssemblyFiles(AssemblyName assemblyName)
            => AssemblySources(assemblyName)
                .SelectMany(source => FindMatchingAssemblyFiles(source, assemblyName));

        private IEnumerable<Directory> AssemblySources(AssemblyName assemblyName)
            => InnerAssemblySources(assemblyName).NotNull();

        private IEnumerable<Directory?> InnerAssemblySources(AssemblyName assemblyName)
        {
            foreach (var projectFolder in _projectFolders.BinaryDirectories)
                yield return projectFolder;

            yield return NugetPackageFolder(assemblyName);

            yield return DotNetSharedFolder();
        }

        private static IEnumerable<File> FindMatchingAssemblyFiles(Directory directory, AssemblyName assemblyName)
            => directory
                .TraverseTree(directory => directory.ChildDirectories)
                .SelectMany(folder => folder.ChildFiles)
                .Where(file => file.Extension == ".dll")
                .Where(file => FileMatchesAssemblyName(assemblyName, file));

        private static bool FileMatchesAssemblyName(AssemblyName assemblyName, File file)
        {
            try
            {
                if (!file.Name.Contains(assemblyName.Name!))
                    return false;
                var fileAssemblyName = AssemblyName.GetAssemblyName(file.Path);
                return fileAssemblyName.Version == assemblyName.Version && fileAssemblyName.Name == assemblyName.Name;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static Directory? NugetPackageFolder(AssemblyName assemblyName)
        {
            try
            {
                return Directory.Create($"{UserFolder}\\.nuget\\packages\\{assemblyName.Name}");
            }
            catch (InvalidFilePath)
            {
                return null;
            }
        }

        private static Directory? DotNetSharedFolder()
        {
            try
            {
                return Directory.Create($"{ProgramFilesFolder}\\dotnet\\shared");
            }
            catch (InvalidFilePath)
            {
                return null;
            }
        }

        private static readonly string UserFolder = Environment.GetEnvironmentVariable("USERPROFILE")!;

        private static readonly string ProgramFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
    }
}
