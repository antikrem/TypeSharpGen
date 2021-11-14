using EphemeralEx.Injection;
using System;

namespace TypeSharpGenLauncher.Core.Synthesiser
{
    [Injectable]
    public interface IEmisionEndpoint
    {
        void Write(string location, string body);
    }

    public class EmisionEndpoint : IEmisionEndpoint
    {
        public void Write(string location, string body)
        {
            Console.WriteLine($"Writing to {location}:");
            Console.Write(body);
        }
    }
}
