using System;
using System.Collections.Generic;

using EphemeralEx.Injection;


namespace TypeSharpGenLauncher.Core.Constructor
{
    [Injectable]
    public interface ITypeScriptBuiltInTypes
    {
        ISet<Type> BuiltInTypes { get; }
        IDictionary<Type, string> BuiltInTypeSymbols { get; }
    }

    public class TypeScriptBuiltInTypes : ITypeScriptBuiltInTypes
    {
        public ISet<Type> BuiltInTypes => new HashSet<Type>(BuiltInTypeSymbols.Keys);

        public IDictionary<Type, string> BuiltInTypeSymbols => new Dictionary<Type, string>()
        {
            { typeof(string), "string" },
            { typeof(int), "number" }
        };

    }
}
