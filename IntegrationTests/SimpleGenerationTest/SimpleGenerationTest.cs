using System.Collections.Generic;

using TypeSharpGen.Builder;


namespace IntegrationTests.PropertiesTest
{
    public class SimpleGenerationTestClass
    {
        public int SimpleProperty { get; }

        public int SimpleMethod(float value)
        {
            return (int)value;
        }
    }

    public class SimpleGenerationTest : IntegrationTest
    {
        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return Declare(typeof(SimpleGenerationTestClass))
                .AddProperty("SimpleProperty")
                .AddMethod("SimpleMethod")
                .EmitTo("SimpleGenerationTest.ts");
        }
    }
}
