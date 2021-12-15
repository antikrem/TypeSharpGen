using System;
using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Extensions;
using EphemeralEx.Injection;

using TypeSharpGen.Builder;
using TypeSharpGenLauncher.Loading;

namespace TypeSharpGenLauncher.Core.Constructor
{
    [Injectable]
    public interface IDependentDefinitionMaterialisation
    {
        IEnumerable<ITypeDefinition> MaterialiseWithDependencies(IEnumerable<ITypeDefinition> typeDefinition);
    }

    public class TypeModelConstructor : IDependentDefinitionMaterialisation
    {
        private readonly ITypeScriptBuiltInTypes _typeScriptBuiltInTypes;
        private readonly ITypeReducer _typeReducer;
        private readonly IAssemblyLoader _assemblyLoader;

        public TypeModelConstructor(ITypeScriptBuiltInTypes typeScriptBuiltInTypes, ITypeReducer typeReducer, IAssemblyLoader assemblyLoader)
        {
            _typeScriptBuiltInTypes = typeScriptBuiltInTypes;
            _typeReducer = typeReducer;
            _assemblyLoader = assemblyLoader;
        }

        public IEnumerable<ITypeDefinition> MaterialiseWithDependencies(IEnumerable<ITypeDefinition> definitions)
        {
            using var context =_assemblyLoader.Context();
            var resolvedDefinition = definitions.ToList();
            var newlyResolved = new List<ITypeDefinition>();

            do
            {
                newlyResolved = ResolutionPass(resolvedDefinition).ToList();
                resolvedDefinition.AddRange(newlyResolved);
            }
            while (newlyResolved.Count > 0);

            return resolvedDefinition.DistinctBy(definition => definition.Type);
        }

        private IEnumerable<ITypeDefinition> ResolutionPass(IReadOnlyList<ITypeDefinition> models)
        {
            var resolvedTypes = new HashSet<Type>(models.Select(model => model.Type));
            return models.SelectMany(model => CreateRequiredModelsForDependencies(model, resolvedTypes));
        }

        private IEnumerable<ITypeDefinition> CreateRequiredModelsForDependencies(ITypeDefinition model, ISet<Type> resolvedTypes)
            => model.DependentTypes()
                .SelectMany(_typeReducer.Reduce)
                .Distinct()
                .Where(dependency => !resolvedTypes.Contains(dependency))
                .Where(dependency => !_typeScriptBuiltInTypes.BuiltInTypes.Contains(dependency))
                .Select(dependency => new DefaultTypeDefinition(dependency, model));
    }
}
