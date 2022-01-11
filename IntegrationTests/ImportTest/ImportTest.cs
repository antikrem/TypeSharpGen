using System.Collections.Generic;

using TypeSharpGen.Builder;


namespace IntegrationTests.Import
{
    public class ImportTestDependencyClass
    {
        public int SimpleProperty { get; }
    }

    public class ImportTestConsumerClass
    {
        public ImportTestDependencyClass DependenyProperty { get; }
    }

    public class ImportTest : IntegrationTest
    {
        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return Declare(typeof(ImportTestDependencyClass))
                .AddProperty("SimpleProperty")
                .EmitTo("ImportTestDependencyClass.ts");

            yield return Declare(typeof(ImportTestConsumerClass))
                .AddProperty("DependenyProperty")
                .EmitTo("ImportTestConsumerClass.ts");
        }
    }
}
