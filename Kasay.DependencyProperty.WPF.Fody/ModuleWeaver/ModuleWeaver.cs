using Fody;
using Kasay.FodyHelpers;
using Mono.Cecil;
using System.Collections.Generic;

public class ModuleWeaver : BaseModuleWeaver
{
    AssemblyFactory baseAssembly;
    AssemblyFactory presentationAssembly;

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
            if (type.InheritFrom("System.Windows.DependencyObject"))
            {
                new ConstructorImplementer(presentationAssembly, type);

                foreach (var prop in type.Properties)
                {
                    if (prop.ExistAttribute("BindAttribute"))
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
