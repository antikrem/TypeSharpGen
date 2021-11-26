using System;
using System.Collections.Generic;
using System.Linq;

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

        public IDictionary<Type, string> BuiltInTypeSymbols
            => InnerBuiltInTypeSymbols()
                .SelectMany(builtIn => DerivedTypes(builtIn.Type, builtIn.Name))
                .ToDictionary(builtIn => builtIn.Type, builtIn => builtIn.Name); //TODO: Compose

        public static IEnumerable<(Type Type, string Name)> InnerBuiltInTypeSymbols()
        {
            yield return (typeof(string), "string");
            yield return (typeof(bool), "boolean");
            yield return (typeof(int), "number");
            yield return (typeof(long), "number");
            yield return (typeof(object), "any");
        }

        private static IEnumerable<(Type Type, string Name)> DerivedTypes(Type type, string name)
        {
            yield return (type, name);
            yield return (type.MakeArrayType(), $"{name}[]");
            yield return (typeof(IEnumerable<>).MakeGenericType(type), $"{name}[]");
            yield return (typeof(IReadOnlyList<>).MakeGenericType(type), $"{name}[]");
            yield return (typeof(List<>).MakeGenericType(type), $"{name}[]");

        }
    }
}
