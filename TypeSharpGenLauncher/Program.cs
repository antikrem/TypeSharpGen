using EphemeralEx.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq.Expressions;
using TypeSharpGenLauncher.Configuration;
using TypeSharpGenLauncher.Generation;
using TypeSharpGenLauncher.Loading;

namespace TypeSharpGenLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ServiceCollection();
            builder.Add(new ServiceDescriptor(typeof(IArgumentsProvider), new ArgumentsProvider(args)));
            builder.AddRegisteredInjections();
            var sp = builder.BuildServiceProvider();

            var genration = sp.GetService<IGeneration>()!;

            genration.Generate();
        }


    }
}
