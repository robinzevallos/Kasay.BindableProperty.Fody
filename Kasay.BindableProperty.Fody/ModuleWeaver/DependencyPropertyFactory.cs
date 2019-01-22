using Kasay.FodyHelpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

internal class DependencyPropertyFactory
{
    readonly AssemblyFactory xamarinAssembly;
    readonly PropertyDefinition propertyDefinition;

    TypeDefinition typeDefinition;
    ModuleDefinition moduleDefinition;
    FieldDefinition dependencyPropertyField;

    public DependencyPropertyFactory(
        AssemblyFactory xamarinAssembly,
        PropertyDefinition propertyDefinition)
    {
        this.xamarinAssembly = xamarinAssembly;
        this.propertyDefinition = propertyDefinition;

        typeDefinition = propertyDefinition.DeclaringType;
        moduleDefinition = typeDefinition.Module;

        AddDependencyPropertyField();
        EqualDependencyPropertyField();
        ModifyGetMethod();
        ModifySetMethod();
    }

    void AddDependencyPropertyField()
    {
        var name = $"{propertyDefinition.Name}Property";

        dependencyPropertyField = new FieldDefinition(
            name,
            FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly,
            xamarinAssembly.GetTypeReference("Xamarin.Forms.BindableProperty"));

        typeDefinition.Fields.Add(dependencyPropertyField);
    }

    void EqualDependencyPropertyField()
    {
        var method = typeDefinition.GetStaticConstructor();

        var nameProperty = propertyDefinition.Name;
        var typeProperty = propertyDefinition.PropertyType;
        var callTypeOf = moduleDefinition.GetMethodReference("System.Type", "GetTypeFromHandle");
        var callCreate = xamarinAssembly.GetMethodReference("Xamarin.Forms.BindableProperty", "Create", 10);

        method.Body.Instructions.RemoveAt(method.Body.Instructions.Count - 1);

        var processor = method.Body.GetILProcessor();
        processor.Emit(OpCodes.Ldstr, nameProperty);
        processor.Emit(OpCodes.Ldtoken, typeProperty);
        processor.Emit(OpCodes.Call, callTypeOf);
        processor.Emit(OpCodes.Ldtoken, typeDefinition);
        processor.Emit(OpCodes.Call, callTypeOf);
        processor.Emit(OpCodes.Ldnull);
        processor.Emit(OpCodes.Ldc_I4_2);
        processor.Emit(OpCodes.Ldnull);
        processor.Emit(OpCodes.Ldnull);
        processor.Emit(OpCodes.Ldnull);
        processor.Emit(OpCodes.Ldnull);
        processor.Emit(OpCodes.Ldnull);
        processor.Emit(OpCodes.Call, callCreate);
        processor.Emit(OpCodes.Stsfld, dependencyPropertyField);
        processor.Emit(OpCodes.Ret);
    }

    void ModifyGetMethod()
    {
        var callGetValue = xamarinAssembly.GetMethodReference("Xamarin.Forms.BindableObject", "GetValue");
        var typeProperty = propertyDefinition.PropertyType;

        propertyDefinition.GetMethod.Body.Instructions.Clear();

        var processor = propertyDefinition.GetMethod.Body.GetILProcessor();
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldsfld, dependencyPropertyField);
        processor.Emit(OpCodes.Call, callGetValue);
        processor.Emit(OpCodes.Unbox_Any, typeProperty);
        processor.Emit(OpCodes.Ret);
    }

    void ModifySetMethod()
    {
        var typeProperty = propertyDefinition.PropertyType;
        var callSetValue = xamarinAssembly.GetMethodReference("Xamarin.Forms.BindableObject", "SetValue");

        propertyDefinition.SetMethod.Body.Instructions.Clear();

        var processor = propertyDefinition.SetMethod.Body.GetILProcessor();
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldsfld, dependencyPropertyField);
        processor.Emit(OpCodes.Ldarg_1);
        processor.Emit(OpCodes.Box, typeProperty);
        processor.Emit(OpCodes.Call, callSetValue);
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ret);
    }
}