using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;

using EphemeralEx.Extensions;
using EphemeralEx.Injection;


namespace TypeSharpGenLauncher.Configuration
{
    [Injectable]
    public interface IConsoleConfiguration
    {
        string ProjectFolder { get; }

        IEnumerable<string> BinaryFolders { get; }
    }

    class ConsoleConfiguration : IConsoleConfiguration
    {
        public string ProjectFolder { private set; get; }

        public IEnumerable<string> BinaryFolders { private set; get; }

        public ConsoleConfiguration(IArgumentsProvider argumentsProvider)
        {
            Resolve(argumentsProvider.Args);
        }

        public int Resolve(IEnumerable<string> args)
        {
            var cmd = CreateCommand();
            cmd.Handler = CommandHandler.Create<FileInfo, string>(Register);
            return cmd.InvokeAsync(args.ToArray()).Result;
        }

        private void Register(FileInfo project_root, string binaries)
        {
            ProjectFolder = project_root.FullName;
            BinaryFolders = binaries.ToEnumerable();
        }

        private static RootCommand CreateCommand()
        {
            var cmd = new RootCommand();
            cmd.AddArgument(new Argument<FileInfo>("project_root", "Root files of the project"));

            foreach (var option in CreateOptions())
                cmd.Add(option);

            return cmd;
        }

        private static IEnumerable<Option> CreateOptions()
        {
            yield return new Option<string>("--binaries", "Optional path to binaries, relative to project_root")
                .Do(option => option.AddAlias("-b"));
        }
    }
}
