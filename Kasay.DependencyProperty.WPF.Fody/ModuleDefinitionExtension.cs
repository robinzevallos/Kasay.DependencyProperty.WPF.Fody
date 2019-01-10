using Fody;
using Mono.Cecil;
using System;
using System.Linq;

public static class ModuleDefinitionExtension
{
    public static TypeReference GetTypeReference(this ModuleDefinition moduleDefinition,  String fullname)
    {
        return moduleDefinition.GetTypeReferences()
            .SingleOrDefault(t => t.FullName == fullname)
            .IfNull($"Type {fullname}");
    }

    public static MethodReference GetMethodReference(this ModuleDefinition moduleDefinition, String fullnameType, String nameMethod)
    {
        var typeReference = moduleDefinition.GetTypeReference(fullnameType);
        var typeDefinition = typeReference.Resolve();
        var methodDefinition = typeDefinition.Methods
            .SingleOrDefault(m => m.Name == nameMethod)
            .IfNull($"Method {nameMethod}");
        var methodReference = typeReference.Module.ImportReference(methodDefinition);

        return methodReference;
    }

    public static T IfNull<T>(this T reference, String message) where T : class
    {
        if (reference is null)
            throw new NullReferenceException($"{message} not found.");

        return reference;
    }

}
