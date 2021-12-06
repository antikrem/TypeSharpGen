using System;
using System.Collections.Generic;
using System.Linq;
using EphemeralEx.Injection;

using TypeSharpGen.Builder;
using TypeSharpGenLauncher.Loading;

namespace TypeSharpGenLauncher.Core.Constructor
{
    [Injectable]
    public interface ITypeModelConstructor
    {
        IEnumerable<ITypeModel> ConstructTypedModels(IEnumerable<ITypeDefinition> typeDefinition);
    }

    public class TypeModelConstructor : ITypeModelConstructor
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

        public IEnumerable<ITypeModel> ConstructTypedModels(IEnumerable<ITypeDefinition> typeModels)
        {
            using var context =_assemblyLoader.Context();
            var resolvedModels = typeModels.Select(model => (ITypeModel)new TypeModel(model)).ToList();
            var newlyResolved = new List<ITypeModel>();

            do
            {
                newlyResolved = ResolutionPass(resolvedModels).ToList();
                resolvedModels.AddRange(newlyResolved);
            }
            while (newlyResolved.Count > 0);

            return resolvedModels;
        }

        private IEnumerable<ITypeModel> ResolutionPass(IReadOnlyList<ITypeModel> models)
        {
            var resolvedTypes = new HashSet<Type>(models.Select(model => model.Type));
            return models.SelectMany(model => CreateRequiredModelsForDependencies(model, resolvedTypes));
        }

        private IEnumerable<ITypeModel> CreateRequiredModelsForDependencies(ITypeModel model, ISet<Type> resolvedTypes)
            => model
                .DependentTypes
                .Select(dependency => _typeReducer.Reduce(dependency))
                .Distinct()
                .Where(dependency => !resolvedTypes.Contains(dependency))
                .Where(dependency => !_typeScriptBuiltInTypes.BuiltInTypes.Contains(dependency))
                .Select(dependency => new DefaultTypeModel(dependency, model));
    }
}
