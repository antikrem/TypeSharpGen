using System.Collections.Generic;

using TypeSharpGen.Builder;


namespace IntegrationTests.SimpleGeneration
{
    public class TriangleImportTestClassA
    {
        public int SimpleProperty { get; }
    }

    public class TriangleImportTestClassB
    {
        public TriangleImportTestClassA Dependent { get; }
    }

    public class TriangleImportTestClassC
    {
        public TriangleImportTestClassA DependentA { get; }
        public TriangleImportTestClassB DependentB { get; }

    }

    public class TriangleImportTest : IntegrationTest
    {
        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return Declare(typeof(TriangleImportTestClassA))
                .AddProperty("SimpleProperty")
                .EmitTo("TriangleImportTestClassA.ts");

            yield return Declare(typeof(TriangleImportTestClassB))
                .AddProperty("Dependent")
                .EmitTo("TriangleImportTestClassB.ts");

            yield return Declare(typeof(TriangleImportTestClassC))
                .AddProperty("DependentA")
                .AddProperty("DependentB")
                .EmitTo("TriangleImportTestClassC.ts");
        }
    }
}
