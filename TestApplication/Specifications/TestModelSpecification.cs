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
            yield return Declare(typeof(TestModel))
                .AddProperty("Name")
                .AddProperty("Child")
                .AddProperty("Child2")
                .AddProperty("Dependent")
                .AddMethod("Something");

            yield return Declare(typeof(DefinedTestSubModel))
                .WithName("AliasedDefinedTestsSubModel")
                .AddProperty("Value2");
        }
    }
}
