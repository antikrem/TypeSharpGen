using System.Collections.Generic;

namespace TypeSharpGenLauncher.Configuration
{
    public interface IArgumentsProvider
    {
        IEnumerable<string> Args { get; }
    }

    public class ArgumentsProvider : IArgumentsProvider
    {
        private readonly string[] _args;

        public ArgumentsProvider(string[] args)
        {
            _args = args;
        }

        public IEnumerable<string> Args => _args;
    }
}
