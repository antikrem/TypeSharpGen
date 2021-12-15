using System.Collections.Generic;
using System.Threading.Tasks;

using TypeSharpGen.Builder;
using TypeSharpGen.Specification;

using TestApplication.Models;


namespace TestApplication.Specifications
{
    class DependentTypeSpecification : GenerationSpecification
    {
        public override string OutputRoot => "Types/MoreSub";

        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return Declare(typeof(DependentType))
                .AddProperty("DependentProperty")
                .AddMethod("Action", new OverideReturnType<Task<int>>())
                .EmitTo("DependentType.d.ts");
        }
    }
}
