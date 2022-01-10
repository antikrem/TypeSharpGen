using System.Collections.Generic;
using System.Linq;

using EphemeralEx.FileSystem;
using EphemeralEx.Injection;


namespace TypeSharpGenLauncher.Configuration
{
    [Injectable]
    public interface IProjectFolders
    {
        Directory ProjectRoot { get; }

        string ProjectRootPath { get; }

        IEnumerable<Directory> BinaryDirectories { get; }
    }

    public class ProjectFolders : IProjectFolders
    {
        private readonly IConsoleConfiguration _consoleConfiguration;

        public ProjectFolders(IConsoleConfiguration consoleConfiguration)
        {
            _consoleConfiguration = consoleConfiguration;
        }

        public Directory ProjectRoot
                => (Directory)IFile.Create(_consoleConfiguration.ProjectFolder);

        public string ProjectRootPath => ProjectRoot.Path;

        public IEnumerable<Directory> BinaryDirectories
                => _consoleConfiguration.BinaryFolders.Select(path => (Directory)IFile.Create(path));
    }
}
