using System.Collections.Generic;

using TypeSharpGen.Builder;
using TypeSharpGen.Specification;

using TestApplication.Models;


namespace TestApplication.Specifications
{
    class TestModelSpecification : GenerationSpecification
    {
        public override string OutputRoot => "Types";

        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return Declare(typeof(TestModel))
                .AddProperty("Name")
                .AddProperty("Name2")
                .AddProperty("Child")
                .AddProperty("Child2")
                .AddProperty("Dependent")
                .AddMethod("Something")
                .EmitTo("types.d.ts");

            yield return Declare(typeof(DefinedTestSubModel))
                .WithName("AliasedDefinedTestsSubModel")
                .AddProperty("Value2")
                .EmitTo("types2.d.ts");
        }
    }
}
