﻿using System;
using System.Collections.Generic;
using System.Linq;
using EphemeralEx.Injection;

using TypeSharpGen.Builder;
using TypeSharpGenLauncher.Core.Model;

namespace TypeSharpGenLauncher.Core.Builder
{
    [Injectable]
    public interface ITypeModelBuilder
    {
        IEnumerable<ITypeModel> ResolveTypedModels(IEnumerable<ITypeDefinition> typeDefinition);
    }

    public class TypeModelBuilder : ITypeModelBuilder
    {
        private readonly ITypeScriptBuiltInTypes _typeScriptBuiltInTypes;

        public TypeModelBuilder(ITypeScriptBuiltInTypes typeScriptBuiltInTypes)
        {
            _typeScriptBuiltInTypes = typeScriptBuiltInTypes;
        }

        public IEnumerable<ITypeModel> ResolveTypedModels(IEnumerable<ITypeDefinition> typeModels)
        {
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
                .Where(dependency => !resolvedTypes.Contains(dependency))
                .Where(dependency => !_typeScriptBuiltInTypes.BuiltInTypes.Contains(dependency))
                .Select(dependency => new DefaultTypeModel(dependency, model));
    }
}
