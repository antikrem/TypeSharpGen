using System;
using System.Collections.Generic;
using TypeSharpGen.Builder;

namespace TypeSharpGen.Specification
{
    public abstract class GenerationSpecification
    {
        public abstract string OutputRoot { get; }

        protected TypeBuilder DeclareInterface(Type type)
            => new(type, Symbol.Interface, OutputRoot);

        protected TypeBuilder DeclareClass(Type type)
            => new(type, Symbol.Class, OutputRoot);

        public abstract IEnumerable<ITypeDefinition> TypeDeclaractions();
    }
}
