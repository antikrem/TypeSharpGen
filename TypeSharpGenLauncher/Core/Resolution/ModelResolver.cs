using EphemeralEx.Injection;
using System;
using System.Collections.Generic;
using System.Linq;

using TypeSharpGen.Builder;
using TypeSharpGenLauncher.Core.Constructor;


namespace TypeSharpGenLauncher.Core.Resolution
{
    [Injectable]
    public interface IModelResolver
    {
        IEnumerable<ITypeModel> ResolveModels(IEnumerable<ITypeDefinition> definitions);
    }

    public class ModelResolver : IModelResolver
    {
        private readonly ITypeReducer _typeReducer;
        private readonly ITypeScriptBuiltInTypes _typeScriptBuiltInTypes;

        public ModelResolver(ITypeReducer typeReducer, ITypeScriptBuiltInTypes typeScriptBuiltInTypes)
        {
            _typeReducer = typeReducer;
            _typeScriptBuiltInTypes = typeScriptBuiltInTypes;
        }

        public IEnumerable<ITypeModel> ResolveModels(IEnumerable<ITypeDefinition> definitions)
        {
            var lookup = definitions.ToDictionary(
                definition => definition.Type, 
                definition => new TypeModel(definition.OutputLocation, definition.Name)
            );

            return definitions.Select(definition => Refine(definition, lookup));
        }

        private TypeModel Refine(ITypeDefinition definition, IReadOnlyDictionary<Type, TypeModel> lookup)
        {
            var typeModel = lookup[definition.Type];

            typeModel.Properties = definition.Properties.Select(property => ResolveProperty(property, lookup)).ToList();
            typeModel.Methods = definition.Methods.Select(method => ResolveMethod(method, lookup)).ToList();
            return typeModel;
        }

        private PropertyModel ResolveProperty(IPropertyDefinition propertyDefinition, IReadOnlyDictionary<Type, TypeModel> lookup) 
            => new(ResolveDerivedSymbol(propertyDefinition.Type, lookup), propertyDefinition.Name);

        private MethodModel ResolveMethod(IMethodDefinition methodDefinition, IReadOnlyDictionary<Type, TypeModel> lookup)
            => new(
                ResolveDerivedSymbol(methodDefinition.ReturnType, lookup),
                ResolveParameters(methodDefinition.Parameters, lookup), 
                methodDefinition.Name
            );

        private IEnumerable<ParameterModel> ResolveParameters(IEnumerable<IParameterDefinition> parameters, IReadOnlyDictionary<Type, TypeModel> lookup)
        {
            return parameters.Select(parameter => new ParameterModel(ResolveDerivedSymbol(parameter.Type, lookup), parameter.Name));
        }

        private IEmmitableSymbol ResolveDerivedSymbol(Type type, IReadOnlyDictionary<Type, TypeModel> lookup)
        {
            if (lookup.TryGetValue(type, out var model))
                return model;

            else if (_typeScriptBuiltInTypes.BuiltInTypes.Contains(type))
                return new DefaultImplementation(_typeScriptBuiltInTypes.BuiltInTypeSymbols[type]);

            else if (_typeReducer.IsReducibleTaskType(type))
                return new TaskTypeModel(ResolveDerivedSymbol(_typeReducer.Reduce(type, 1).Single(), lookup));

            else if (_typeReducer.IsReducibleListType(type))
                return new ArrayTypeModel(ResolveDerivedSymbol(_typeReducer.Reduce(type, 1).Single(), lookup));

            else if (_typeReducer.IsReducibleDictionaryType(type))
            {
                var parameters = _typeReducer.Reduce(type, 1).ToList();
                return new DictionaryTypeModel(
                    ResolveDerivedSymbol(parameters[0], lookup),
                    ResolveDerivedSymbol(parameters[1], lookup)
                );

            }

            else throw new Exception();
        }
    }
}
