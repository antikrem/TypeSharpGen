using System;
using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Extensions;
using EphemeralEx.Injection;


namespace TypeSharpGenLauncher.Core.Constructor
{
    [Injectable]
    public interface ITypeReducer
    {
        IEnumerable<Type> IterableTypes { get; }
        IDictionary<Type, string> ComposedTypes(Type type, string name);
        Type Reduce(Type type);
    }

    public class TypeReducer : ITypeReducer
    {
        public Type Reduce(Type type)
        {
            if (type.IsArray)
                return Reduce(type.GetElementType()!);
            else if (IsReducibleGeneric(type))
                return Reduce(type.GetGenericArguments().First());

            return type;
        }

        public IDictionary<Type, string> ComposedTypes(Type type, string name)
            => DerivedTypes(type, name).Compose();

        public IEnumerable<Type> IterableTypes 
        { 
            get
            {
                yield return (typeof(IEnumerable<>));
                yield return (typeof(IReadOnlyList<>));
                yield return (typeof(List<>));
            }
        }

        private bool IsReducibleGeneric(Type type)
            => type.IsGenericType && IterableTypes.Contains(type.GetGenericTypeDefinition());

        private static IEnumerable<(Type Type, string Name)> DerivedTypes(Type type, string name)
        {
            yield return (type, name);
            yield return (type.MakeArrayType(), $"{name}[]");
            if (type != typeof(void))
            {
                yield return (typeof(IEnumerable<>).MakeGenericType(type), $"{name}[]");
                yield return (typeof(IReadOnlyList<>).MakeGenericType(type), $"{name}[]");
                yield return (typeof(List<>).MakeGenericType(type), $"{name}[]");
            }
        }

    }
}
