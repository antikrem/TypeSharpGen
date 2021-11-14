using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EphemeralEx.Extensions;
using EphemeralEx.Injection;

using TypeSharpGen.Builder;
using TypeSharpGenEmitter.Core.Model;

namespace TypeSharpGenEmitter.Core.Builder
{
    [Injectable]
    public interface ITypeModelBuilder
    {
        IEnumerable<ITypeModel> ResolveTypedModels(IEnumerable<ITypeDefinition> typeDefinition);
    }

    public class TypeModelBuilder : ITypeModelBuilder
    {
        public IEnumerable<ITypeModel> ResolveTypedModels(IEnumerable<ITypeDefinition> typeModels)
        {
            var resolvedModels = typeModels.Select(model => (ITypeModel) new TypeModel(model)).ToList();
            var newlyResolved = new List<ITypeModel>();

            do
            {
                newlyResolved = ResolutionPass(resolvedModels).ToList();
                resolvedModels.AddRange(newlyResolved);
            }
            while (newlyResolved.Count > 0);

            return resolvedModels;
        }

        private static IEnumerable<ITypeModel> ResolutionPass(IReadOnlyList<ITypeModel> models)
        {
            var resolvedTypes = new SortedSet<Type>(models.Select(model => model.Type));
            return models.SelectMany(model => CreateRequiredModelsForDependencies(model, resolvedTypes));
        }

        private static IEnumerable<ITypeModel> CreateRequiredModelsForDependencies(ITypeModel model, ISet<Type> resolvedTypes) 
            => model
                .DependentTypes
                .Where(dependency => !resolvedTypes.Contains(dependency))
                .Select(dependency => new DefaultTypeModel(dependency, model));
    }
}
