using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System.Linq;

partial class ModuleWeaver
{
    void DependencyPropertyFactory(TypeDefinition typeDefinition)
    {
        AddDependencyPropertyField(typeDefinition);
        EqualDependencyPropertyField(typeDefinition);
        //EqualDataContextInCtor(typeDefinition);
    }

    void AddDependencyPropertyField(TypeDefinition typeDefinition)
    {
        var nameField = $"DemoProperty";

        dependencyPropertyField = new FieldDefinition(
            nameField,
            FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly,
            wpfAssembly.GetTypeReference("System.Windows.DependencyProperty"));

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
        var callRegister = wpfAssembly.GetMethodReference("System.Windows.DependencyProperty", "Register", 3);

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
        var callGet_Content = wpfAssembly.GetMethodReference("Windows.UI.Xaml.Controls.UserControl", "get_Content");
        var frameworkElement = wpfAssembly.GetTypeReference("Windows.UI.Xaml.FrameworkElement");
        var callPut_DataContext = wpfAssembly.GetMethodReference("Windows.UI.Xaml.FrameworkElement", "put_DataContext");

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
}
