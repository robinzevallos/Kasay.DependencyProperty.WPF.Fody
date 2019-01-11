using Fody;
using Kasay.DependencyProperty.WPF.Fody;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

public partial class ModuleWeaver : BaseModuleWeaver
{
    AssemblyFactory baseAssembly;
    AssemblyFactory presentationAssembly;

    public override void Execute()
    {
        SetAssemblies();
        SetDependencyProperties();
    }

    void SetAssemblies()
    {
        baseAssembly = new AssemblyFactory("WindowsBase", ModuleDefinition);
        presentationAssembly = new AssemblyFactory("PresentationFramework", ModuleDefinition);
    }

    void SetDependencyProperties()
    {
        foreach (var type in ModuleDefinition.GetTypes())
        {
            var isTargetType = type.CustomAttributes
                .Any(_ => _.AttributeType.Name == "AutoDependencyPropertyAttribute");

            if (isTargetType)
                DependencyPropertyFactory(type);
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }
}
