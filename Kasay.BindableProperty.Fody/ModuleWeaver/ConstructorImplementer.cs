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

    public ConstructorImplementer(
        AssemblyFactory xamarinAssembly,
        TypeDefinition typeDefinition,
        Boolean isModeTest)
    {
        this.xamarinAssembly = xamarinAssembly;
        this.typeDefinition = typeDefinition;

        moduleDefinition = typeDefinition.Module;

        if (!isModeTest)
            EqualDataContext();

        AddStaticConstructor();
    }

    void EqualDataContext()
    {
        var method = typeDefinition.GetConstructors().First();

        var callGet_Content = xamarinAssembly.GetMethodReference("Xamarin.Forms.ContentView", "get_Content");
        var callPut_BindingContext = xamarinAssembly.GetMethodReference("Xamarin.Forms.BindableObject", "set_BindingContext");

        method.Body.Instructions.RemoveAt(method.Body.Instructions.Count - 1);

        var processor = method.Body.GetILProcessor();
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Call, callGet_Content);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Callvirt, callPut_BindingContext);
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
