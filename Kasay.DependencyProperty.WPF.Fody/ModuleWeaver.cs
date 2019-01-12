using Fody;
using Kasay.DependencyProperty.WPF.Fody;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

public class ModuleWeaver : BaseModuleWeaver
{
    readonly Boolean isModeTest;

    AssemblyFactory baseAssembly;
    AssemblyFactory presentationAssembly;

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
                new ConstructorImplementer(presentationAssembly, type, isModeTest);

                foreach (var prop in type.Properties)
                {
                    new DependencyPropertyFactory(baseAssembly, prop);
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
