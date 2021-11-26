using EphemeralEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace TypeSharpGen.Builder
{
    public interface IParameterDefinition
    {
        Type Type { get; }

        string Name { get; }
    }

    public class ParameterDefinition : IParameterDefinition
    {
        private readonly ParameterInfo _parameterInfo;

        public Type Type => _parameterInfo.ParameterType;

        public string Name => _parameterInfo.Name ?? throw new Exception(); //TODO: ???

        public ParameterDefinition(ParameterInfo parameterInfo)
        {
            _parameterInfo = parameterInfo;
        }
    }

    public interface IMethodDefinition
    {
        Type ReturnType { get; }

        string Name { get; }

        IEnumerable<IParameterDefinition> Parameters { get; }

        IEnumerable<Type> Dependencies
            => Parameters
                .Select(parameter => parameter.Type)
                .Concat(ReturnType.ToEnumerable())
                .Distinct();
    }

    public class MethodDefinition : IMethodDefinition
    {
        private readonly MethodInfo _methodInfo;

        public Type ReturnType => _methodInfo.ReturnType;

        public string Name => _methodInfo.Name;

        public IEnumerable<IParameterDefinition> Parameters
            => _methodInfo
                .GetParameters()
                .Select(paramter => new ParameterDefinition(paramter));

        public MethodDefinition(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }
    }
}
