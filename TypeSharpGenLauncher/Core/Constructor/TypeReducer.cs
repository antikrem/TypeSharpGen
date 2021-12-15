using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EphemeralEx.Extensions;
using EphemeralEx.Injection;


namespace TypeSharpGenLauncher.Core.Constructor
{
    [Injectable]
    public interface ITypeReducer
    {
        bool IsReducibleListType(Type type);
        bool IsReducibleDictionaryType(Type type);
        bool IsReducibleTaskType(Type type);

        IEnumerable<Type> Reduce(Type type, int by = int.MaxValue);
    }

    public class TypeReducer : ITypeReducer
    {
        public IEnumerable<Type> Reduce(Type type, int by)
        {
            if (by <= 0)
                return type.ToEnumerable();
            else if (IsReducibleTaskType(type))
                return Reduce(type.GetGenericArguments().Single(), by - 1);
            else if (type.IsArray)
                return Reduce(type.GetElementType()!, by - 1);
            else if (IsReducibleListType(type))
                return Reduce(type.GetGenericArguments().First(), by - 1);
            else if (IsReducibleDictionaryType(type))
                return type
                    .GetGenericArguments()
                    .Take(2)
                    .SelectMany(argument => Reduce(argument, by - 1));

            return type.ToEnumerable();
        }

        public bool IsReducibleListType(Type type)
            => type.IsArray || type.IsGenericType && IterableTypes.Contains(type.GetGenericTypeDefinition());

        public bool IsReducibleDictionaryType(Type type)
            => type.IsGenericType && DictionaryTypes.Contains(type.GetGenericTypeDefinition());

        public bool IsReducibleTaskType(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);

        private static IEnumerable<Type> IterableTypes
        {
            get
            {
                yield return (typeof(IEnumerable<>));
                yield return (typeof(IReadOnlyList<>));
                yield return (typeof(List<>));
            }
        }

        private static IEnumerable<Type> DictionaryTypes
        {
            get
            {
                yield return (typeof(IReadOnlyDictionary<,>));
                yield return (typeof(Dictionary<,>));
            }
        }
    }
}
