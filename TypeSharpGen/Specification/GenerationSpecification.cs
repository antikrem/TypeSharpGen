using System;
using System.Collections.Generic;
using TypeSharpGen.Builder;

namespace TypeSharpGen.Specification
{
    public abstract class GenerationSpecification
    {
        public abstract string OutputRoot { get; }

        protected TypeBuilder Declare(Type type)
            => new(type, OutputRoot);

        public abstract IEnumerable<ITypeDefinition> TypeDeclaractions();
    }
}
