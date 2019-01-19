using Fody;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

public class ModuleWeaver : BaseModuleWeaver
{
    readonly Boolean isModeTest;

    AssemblyFactory xamarinAssembly;

    public ModuleWeaver()
    {
    }

    public ModuleWeaver(Boolean isModeTest) : this()
    {
        this.isModeTest = isModeTest;
    }

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
            var isTargetType = type.CustomAttributes
                .Any(_ => _.AttributeType.Name == "AutoBindablePropertyAttribute");

            if (isTargetType)
            {
                new ConstructorImplementer(xamarinAssembly, type, isModeTest);

                foreach (var prop in type.Properties)
                {
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
