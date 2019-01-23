using Fody;
using Kasay.FodyHelpers;
using Mono.Cecil;
using System.Collections.Generic;

public class ModuleWeaver : BaseModuleWeaver
{
    AssemblyFactory xamarinAssembly;

    public override void Execute()
    {
        SetAssemblies();
        SetDependencyPropertyToTypes();
    }

    void SetAssemblies()
    {
        xamarinAssembly = new AssemblyFactory("Xamarin.Forms.Core", ModuleDefinition);
    }

    void SetDependencyPropertyToTypes()
    {
        foreach (var type in ModuleDefinition.GetTypes())
        {         
            if (type.InheritFrom("Xamarin.Forms.BindableObject"))
            {
                new ConstructorImplementer(xamarinAssembly, type);

                foreach (var prop in type.Properties)
                {
                    if (prop.ExistAttribute("BindAttribute"))
                        new DependencyPropertyFactory(xamarinAssembly,  prop);
                }
            }
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }
}
