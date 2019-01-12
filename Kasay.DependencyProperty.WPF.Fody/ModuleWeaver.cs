using Fody;
using Kasay.DependencyProperty.WPF.Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ModuleWeaver : BaseModuleWeaver
{
    AssemblyFactory baseAssembly;
    AssemblyFactory presentationAssembly;

    Boolean isModeTest;

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
        baseAssembly = new AssemblyFactory("WindowsBase", ModuleDefinition);
        presentationAssembly = new AssemblyFactory("PresentationFramework", ModuleDefinition);
    }

    void SetDependencyPropertyToTypes()
    {
        foreach (var type in ModuleDefinition.GetTypes())
        {
            var isTargetType = type.CustomAttributes
                .Any(_ => _.AttributeType.Name == "AutoDependencyPropertyAttribute");

            if (isTargetType)
            {
                if (!isModeTest)
                    EqualDataContextInCtor(type);

                AddStaticConstructor(type);

                foreach (var prop in type.Properties)
                {
                    DependencyPropertyFactory(type, prop);
                }
            }
        }
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

    void AddStaticConstructor(TypeDefinition typeDefinition)
    {
        var method = typeDefinition.GetStaticConstructor();

        if (method is null)
        {
            method = new MethodDefinition(
                ".cctor",
                MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName | MethodAttributes.Static,
                TypeSystem.VoidReference);

            var processor = method.Body.GetILProcessor();
            processor.Emit(OpCodes.Ret);

            typeDefinition.Methods.Add(method);
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }
}
