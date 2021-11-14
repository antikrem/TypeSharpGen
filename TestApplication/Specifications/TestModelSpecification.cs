using System.Collections.Generic;

using TypeSharpGen.Builder;
using TypeSharpGen.Specification;

using TestApplication.Models;


namespace TestApplication.Specifications
{
    class TestModelSpecification : GenerationSpecification
    {
        public override string OutputRoot => "TestSpecification";

        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return DeclareInterface(typeof(TestModel))
                .AddProperty("Name")
                .AddProperty("Child");
        }
    }
}
