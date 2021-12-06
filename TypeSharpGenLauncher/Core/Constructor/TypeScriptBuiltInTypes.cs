using System;
using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Extensions;
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
        private readonly ITypeReducer _typeReducer;

        public TypeScriptBuiltInTypes(ITypeReducer typeReducer)
        {
            _typeReducer = typeReducer;
        }

        public ISet<Type> BuiltInTypes => new HashSet<Type>(BuiltInTypeSymbols.Keys);

        public IDictionary<Type, string> BuiltInTypeSymbols
            => InnerBuiltInTypeSymbols()
                .SelectMany(builtIn => _typeReducer.ComposedTypes(builtIn.Type, builtIn.Name))
                .Compose();

        public static IEnumerable<(Type Type, string Name)> InnerBuiltInTypeSymbols()
        {
            yield return (typeof(void), "void");
            yield return (typeof(string), "string");
            yield return (typeof(bool), "boolean");
            yield return (typeof(int), "number");
            yield return (typeof(long), "number");
            yield return (typeof(float), "number");
            yield return (typeof(double), "number");
            yield return (typeof(object), "any");
        }
    }
}
