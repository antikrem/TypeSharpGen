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
        IEnumerable<Type> DictionaryTypes { get; }
        IDictionary<Type, string> ComposedTypes(Type type, string name);
        IEnumerable<Type> Reduce(Type type);
    }

    public class TypeReducer : ITypeReducer
    {
        public IEnumerable<Type> Reduce(Type type)
        {
            if (type.IsArray)
                return Reduce(type.GetElementType()!);
            else if (IsReducibleListGeneric(type))
                return Reduce(type.GetGenericArguments().First());
            else if (IsReducibleDictionaryGeneric(type))
                return type
                    .GetGenericArguments()
                    .Take(2)
                    .SelectMany(argument => Reduce(argument));

            return type.ToEnumerable();
        }

        public IDictionary<Type, string> ComposedTypes(Type type, string name)
            => DerivedTypes(type, name).Compose();

        private bool IsReducibleListGeneric(Type type)
            => type.IsGenericType && IterableTypes.Contains(type.GetGenericTypeDefinition());

        public IEnumerable<Type> IterableTypes
        {
            get
            {
                yield return (typeof(IEnumerable<>));
                yield return (typeof(IReadOnlyList<>));
                yield return (typeof(List<>));
            }
        }

        private bool IsReducibleDictionaryGeneric(Type type)
            => type.IsGenericType && DictionaryTypes.Contains(type.GetGenericTypeDefinition());

        public IEnumerable<Type> DictionaryTypes
        {
            get
            {
                yield return (typeof(IReadOnlyDictionary<,>));
                yield return (typeof(Dictionary<,>));
            }
        }

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
