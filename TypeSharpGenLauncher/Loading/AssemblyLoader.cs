using System;
using System.Reflection;

using EphemeralEx.Injection;


namespace TypeSharpGenLauncher.Loading
{
    public class AssemblyLoaderContext : IDisposable
    {
        private readonly AssemblyLoader _assemblyLoader;

        internal AssemblyLoaderContext(AssemblyLoader assemblyLoader)
        {
            _assemblyLoader = assemblyLoader;
        }

        public void Dispose()
        {
            _assemblyLoader.Deregister();
        }
    }

    [Injectable]
    public interface IAssemblyLoader
    {
        public AssemblyLoaderContext Context();
    }

    public class AssemblyLoader : IAssemblyLoader
    {
        private readonly IAssemblyFileFinder _assemblyFileFinder;

        public AssemblyLoader(IAssemblyFileFinder assemblyFileProvider)
        {
            _assemblyFileFinder = assemblyFileProvider;
        }

        public AssemblyLoaderContext Context()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(SearchingAssemblyResolutionStrategy);

            return new AssemblyLoaderContext(this);
        }

        internal void Deregister()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(SearchingAssemblyResolutionStrategy);
        }

        Assembly SearchingAssemblyResolutionStrategy(object? sender, ResolveEventArgs args)
        {
            var requestedName = new AssemblyName(args.Name);
            Console.WriteLine($"Trying to load {requestedName.Name}, {requestedName.Version}");

            foreach (var file in _assemblyFileFinder.FindMacthingAssemblyFiles(requestedName))
            {
                try
                {
                    var assmebly = Assembly.LoadFile(file.Path);
                    Console.WriteLine($"Loaded {assmebly.GetName()}, {assmebly.ImageRuntimeVersion} from {file.Path}");
                    return assmebly;
                }
                catch (Exception)
                { }
            }

            throw new Exception(); // TODO
        }


    }
}
