# TypeSharpGen

**C# to TypeScript Declaration Generator**

Generate typescript from C# with simple syntax and a non-opinionated, extendable design. Loosely inspired by `TypeGen`, this project offers more features such as method declarations as well as more low level customization.

## Packages

* [NuGet](https://www.nuget.org/packages/TypeSharpGen/)
* [NuGet - CLI tool](https://www.nuget.org/packages/TypeSharpGenLauncher/)

## Usage

1. Add the [NuGet](https://www.nuget.org/packages/TypeSharpGen/) package to your project. 

2. Implement `GenerationSpecification` to define the declaration process.

3. Install the [NuGet - CLI tool](https://www.nuget.org/packages/TypeSharpGenLauncher/) package as a dotnet cli tool.

4. Run the tool with `typesharpgenerate [Project Root] -n [Binary Folder]`

## Example
A useful application is to create type definitions for your client side application in ASP.NET:

#### C# file
```c#
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
    
public class ControllerAndTypesGenerator : GenerationSpecification
{
    public override string OutputRoot => "Typings";

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
```

Running the CLI tool gets:

#### Typings\ViewModel.ts
```typescript
// This file has been generated by TypeSharpGen

export interface ViewModel {
    Property: Dependency;
}

export interface Dependency {
    SimpleProperty: number;
}
```

#### Typings\Controller.ts
```typescript
// This file has been generated by TypeSharpGen
import { ViewModel } from "./ViewModel";

export interface Controller {

    Action(parameter: number): Promise<ViewModel>;
}
```

With some extra work, its possible to automate the declaration of controllers (TODO - pull the reflective generation specification from TagH to here).
