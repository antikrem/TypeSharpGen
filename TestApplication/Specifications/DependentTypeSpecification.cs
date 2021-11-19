using System.Collections.Generic;

using TypeSharpGen.Builder;
using TypeSharpGen.Specification;

using TestApplication.Models;


namespace TestApplication.Specifications
{
    class DependentTypeSpecification : GenerationSpecification
    {
        public override string OutputRoot => "DependentType";

        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return DeclareInterface(typeof(DependentType))
                .AddProperty("DependentProperty");
        }
    }
}
