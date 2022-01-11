using System.Collections.Generic;
using System.Threading.Tasks;
using TypeSharpGen.Builder;


namespace IntegrationTests.Controller
{
    public class Dependency
    {
        public int SimpleProperty { get; }
    }

    public class ViewModel
    {
        public Dependency Property { get; }
    }

    public class Controller
    {
        public ViewModel Action(int parameter) => new ViewModel();
    }

    public class ControllerTest : IntegrationTest
    {
        public override IEnumerable<ITypeDefinition> TypeDeclaractions()
        {
            yield return Declare(typeof(ViewModel))
                .AddProperty("Property")
                .EmitTo("ViewModel.ts");

            yield return Declare(typeof(Controller))
                .AddMethod("Action", new OverideReturnType<Task<ViewModel>>())
                .EmitTo("Controller.ts");
        }
    }
}
