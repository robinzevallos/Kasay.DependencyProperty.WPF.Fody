using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System.Linq;

partial class ModuleWeaver
{
    FieldDefinition dependencyPropertyField;
    PropertyDefinition targetPropertyDefinition;

    void DependencyPropertyFactory(TypeDefinition typeDefinition)
    {
        AddDependencyPropertyField(typeDefinition);
        EqualDependencyPropertyField(typeDefinition);
        //EqualDataContextInCtor(typeDefinition);
        GetProperty(typeDefinition);
        ModifyGetMethod();
        ModifySetMethod();
    }

    void AddDependencyPropertyField(TypeDefinition typeDefinition)
    {
        var nameField = $"DemoProperty";

        dependencyPropertyField = new FieldDefinition(
            nameField,
            FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly,
            baseAssembly.GetTypeReference("System.Windows.DependencyProperty"));

        typeDefinition.Fields.Add(dependencyPropertyField);
    }

    void EqualDependencyPropertyField(TypeDefinition typeDefinition)
    {
        var method = new MethodDefinition(
            ".cctor",
            MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName | MethodAttributes.Static,
            TypeSystem.VoidReference);

        var callTypeOf = ModuleDefinition.GetMethodReference("System.Type", "GetTypeFromHandle");
        var callRegister = baseAssembly.GetMethodReference("System.Windows.DependencyProperty", "Register", 3);

        var processor = method.Body.GetILProcessor();
        processor.Emit(OpCodes.Ldstr, "Demo");
        processor.Emit(OpCodes.Ldtoken, TypeSystem.StringReference);
        processor.Emit(OpCodes.Call, callTypeOf);
        processor.Emit(OpCodes.Ldtoken, typeDefinition);
        processor.Emit(OpCodes.Call, callTypeOf);
        processor.Emit(OpCodes.Call, callRegister);
        processor.Emit(OpCodes.Stsfld, dependencyPropertyField);
        processor.Emit(OpCodes.Ret);

        typeDefinition.Methods.Add(method);
    }

    void EqualDataContextInCtor(TypeDefinition typeDefinition)
    {
        var method = typeDefinition.GetConstructors().First();

        var callGet_Content = presentationAssembly.GetMethodReference("System.Windows.Controls.ContentControl", "get_Content");
        var frameworkElement = presentationAssembly.GetTypeReference("System.Windows.FrameworkElement");
        var callPut_DataContext = presentationAssembly.GetMethodReference("System.Windows.FrameworkElement", "set_DataContext");

        method.Body.Instructions.RemoveAt(method.Body.Instructions.Count - 1);

        var processor = method.Body.GetILProcessor();
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Call, callGet_Content);
        processor.Emit(OpCodes.Castclass, frameworkElement);
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Callvirt, callPut_DataContext);
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ret);
    }

    void GetProperty(TypeDefinition typeDefinition)
    {
        targetPropertyDefinition = typeDefinition.Properties.Single(_ => _.Name == "Demo");
    }

    void ModifyGetMethod()
    {
        var callGetValue = baseAssembly.GetMethodReference("System.Windows.DependencyObject", "GetValue");

        targetPropertyDefinition.GetMethod.Body.Instructions.Clear();

        var processor = targetPropertyDefinition.GetMethod.Body.GetILProcessor();
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldsfld, dependencyPropertyField);
        processor.Emit(OpCodes.Call, callGetValue);
        processor.Emit(OpCodes.Castclass, TypeSystem.StringReference);
        processor.Emit(OpCodes.Ret);
    }

    void ModifySetMethod()
    {
        var callSetValue = baseAssembly.GetMethodReference("System.Windows.DependencyObject", "SetValue");

        targetPropertyDefinition.SetMethod.Body.Instructions.Clear();

        var processor = targetPropertyDefinition.SetMethod.Body.GetILProcessor();
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldsfld, dependencyPropertyField);
        processor.Emit(OpCodes.Ldarg_1);
        processor.Emit(OpCodes.Call, callSetValue);
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ret);
    }

}
