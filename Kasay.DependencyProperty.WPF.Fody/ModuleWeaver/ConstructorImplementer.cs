using Kasay.FodyHelpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System.Linq;

internal class ConstructorImplementer
{
    readonly AssemblyFactory presentationAssembly;
    readonly TypeDefinition typeDefinition;
    readonly ModuleDefinition moduleDefinition;

    public ConstructorImplementer(AssemblyFactory presentationAssembly, TypeDefinition typeDefinition)
    {
        this.presentationAssembly = presentationAssembly;
        this.typeDefinition = typeDefinition;
        moduleDefinition = typeDefinition.Module;

        EqualDataContext();
        AddStaticConstructor();
    }

    void EqualDataContext()
    {
        if (typeDefinition.InheritFrom("System.Windows.FrameworkElement"))
        {
            var method = typeDefinition.GetConstructors().First();

            var callSet_DataContext = presentationAssembly.GetMethodReference("System.Windows.FrameworkElement", "set_DataContext");

            method.Body.Instructions.RemoveAt(method.Body.Instructions.Count - 1);

            var processor = method.Body.GetILProcessor();
            processor.Emit(OpCodes.Nop);
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Call, callSet_DataContext);
            processor.Emit(OpCodes.Nop);
            processor.Emit(OpCodes.Ret);
        }
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
