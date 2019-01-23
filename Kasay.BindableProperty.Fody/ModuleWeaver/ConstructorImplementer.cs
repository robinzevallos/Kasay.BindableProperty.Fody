using Kasay.FodyHelpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Linq;

internal class ConstructorImplementer
{
    readonly AssemblyFactory xamarinAssembly;
    readonly TypeDefinition typeDefinition;

    ModuleDefinition moduleDefinition;

    public ConstructorImplementer(AssemblyFactory xamarinAssembly, TypeDefinition typeDefinition)
    {
        this.xamarinAssembly = xamarinAssembly;
        this.typeDefinition = typeDefinition;

        moduleDefinition = typeDefinition.Module;

        EqualBindingContext();
        AddStaticConstructor();
    }

    void EqualBindingContext()
    {
        var method = typeDefinition.GetConstructors().First();

        var callPut_BindingContext = xamarinAssembly.GetMethodReference("Xamarin.Forms.BindableObject", "set_BindingContext");

        method.Body.Instructions.RemoveAt(method.Body.Instructions.Count - 1);

        var processor = method.Body.GetILProcessor();
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Call, callPut_BindingContext);
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ret);
    }

    void AddStaticConstructor()
    {
        var method = typeDefinition.GetStaticConstructor();

        if (method is null)
        {
            method = new MethodDefinition(
                ".cctor",
                MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName | MethodAttributes.Static,
                moduleDefinition.TypeSystem.Void);

            var processor = method.Body.GetILProcessor();
            processor.Emit(OpCodes.Ret);

            typeDefinition.Methods.Add(method);
        }
    }
}
