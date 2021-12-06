using System;
using System.Text;

using EphemeralEx.Injection;
using EphemeralEx.FileSystem;


namespace TypeSharpGenLauncher.Core.Synthesiser
{
    [MultipleInjectable]
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

    public class FileEndpoint : IEmisionEndpoint
    {
        public void Write(string location, string body)
        {
            File.CreateNew(location, Encoding.ASCII.GetBytes(body));
        }
    }
}
