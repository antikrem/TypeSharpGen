using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EphemeralEx.Injection;


namespace TypeSharpGenLauncher.Core.Constructor
{
    [Injectable]
    public interface ITypeScriptBuiltInTypes
    {
        ISet<Type> BuiltInTypes { get; }
        IReadOnlyDictionary<Type, string> BuiltInTypeSymbols { get; }
    }

    public class TypeScriptBuiltInTypes : ITypeScriptBuiltInTypes
    {
        public ISet<Type> BuiltInTypes => new HashSet<Type>(BuiltInTypeSymbols.Keys);

        public IReadOnlyDictionary<Type, string> BuiltInTypeSymbols
            => new Dictionary<Type, string>
            {
                { typeof(void), "void" },
                { typeof(string), "string" },
                { typeof(bool), "boolean" },
                { typeof(int), "number" },
                { typeof(long), "number" },
                { typeof(float), "number" },
                { typeof(double), "number" },
                { typeof(object), "any" },
                { typeof(Task), "Promise<void>" }
            };
    }
}
