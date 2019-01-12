namespace Kasay.DependencyProperty.WPF.Fody
{
    using Mono.Cecil;
    using System;
    using System.Linq;

    internal class AssemblyFactory
    {
        readonly ModuleDefinition moduleDefinition;

        AssemblyDefinition assemblydefinition;

        public AssemblyFactory(String name, ModuleDefinition moduleDefinition)
        {
            this.moduleDefinition = moduleDefinition;

            SetAssembly(name);
        }  

        public TypeReference GetTypeReference(String fullname)
        {
            var typeDefinition = GetTypeDefinition(fullname);
            var typeReference = GetTypeReference(typeDefinition);

            return typeReference;
        }

        public MethodReference GetMethodReference(
            String fullnameType,
            String nameMethod,
            Int32? numberParameters = null)
        {
            var typeDefinition = GetTypeDefinition(fullnameType);
            var typeReference = GetTypeReference(typeDefinition);

            MethodDefinition methodDefinition;

            if (numberParameters is null)
            {
                methodDefinition = typeDefinition.Methods
                    .Where(m => m.Name == nameMethod)
                    .FirstOrDefault();
            }
            else
            {
                methodDefinition = typeDefinition.Methods
                    .Where(m => m.Name == nameMethod && m.Parameters.Count == numberParameters)
                    .FirstOrDefault();
            }

            methodDefinition.IfNull($"Method {nameMethod}");

            var methodReference = typeReference.Module.ImportReference(methodDefinition);

            return methodReference;
        }

        void SetAssembly(String name)
        {
            var assemblyReference = new AssemblyNameReference(name, Version.Parse("0.0.0.0"));
            assemblydefinition = moduleDefinition.AssemblyResolver.Resolve(assemblyReference)
                .IfNull($"Assembly {assemblyReference.Name}");
        }

        TypeReference GetTypeReference(TypeDefinition typeDefinition)
        {
            return moduleDefinition.ImportReference(typeDefinition);
        }

        TypeDefinition GetTypeDefinition(String fullname)
        {
            return assemblydefinition.MainModule.GetTypes()
                .SingleOrDefault(_ => _.FullName == fullname)
                .IfNull($"Type {fullname}");
        }
    }
}
