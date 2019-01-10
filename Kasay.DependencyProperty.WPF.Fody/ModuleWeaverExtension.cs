using Fody;
using Mono.Cecil;
using System;
using System.Linq;

public static class ModuleWeaverExtension
{
    public static TypeReference GetTypeReference(this BaseModuleWeaver @base,  String fullnameType)
    {
        var typeReference = @base.ModuleDefinition.GetTypeReferences()
            .Single(t => t.FullName == fullnameType);

        return typeReference;
    }

    public static MethodReference GetMethodReference(this BaseModuleWeaver @base, String fullnameType, String nameMethod)
    {
        var typeReference = @base.GetTypeReference(fullnameType);
        var typeDefinition = typeReference.Resolve();
        var methodDefinition = typeDefinition.Methods
            .Single(m => m.Name == nameMethod);
        var methodReference = typeReference.Module.ImportReference(methodDefinition);

        return methodReference;
    }
}
