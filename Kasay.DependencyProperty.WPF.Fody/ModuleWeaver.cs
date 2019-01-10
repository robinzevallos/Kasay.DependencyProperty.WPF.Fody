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
    private FieldDefinition dependencyPropertyField;
    private PropertyDefinition targetPropertyDefinition;

    AssemblyFactory wpfAssembly, customAssembly;

    public override void Execute()
    {
        SetAssembly();

        var types = ModuleDefinition.GetTypes().Where(_ => _.Name.EndsWith("PUTO"));

        foreach (var type in types)
        {
            DependencyPropertyFactory(type);
            //AddField(type);
            //AddEqualField(type);

            //GetProperty(type);

            //ModifyGetMethod();
            //ModifySetMethod();

            //ModifyCtor(type);
        }
    }

    void SetAssembly()
    {
        wpfAssembly = new AssemblyFactory("WindowsBase", ModuleDefinition);
        //customAssembly = new AssemblyFactory("Kasay.DependencyProperty.UWP", ModuleDefinition);
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
        //yield return "WindowsBase";

    }
}
