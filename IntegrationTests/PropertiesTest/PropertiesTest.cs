using System.Collections.Generic;

using TypeSharpGen.Builder;


namespace IntegrationTests.PropertiesTest
{
    public class PropertyTestClass
    {
        public int SimpleProperty { get; }
    }

    public class PropertiesTest : IntegrationTest
    {
        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return Declare(typeof(PropertyTestClass))
                .AddProperty("SimpleProperty")
                .EmitTo("PropertyTestClass.ts");
        }
    }
}
