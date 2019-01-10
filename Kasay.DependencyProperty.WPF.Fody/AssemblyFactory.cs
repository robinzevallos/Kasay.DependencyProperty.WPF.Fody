namespace Kasay.DependencyProperty.WPF.Fody
{
    using Mono.Cecil;
    using System;
    using System.Linq;
    using Mono.Cecil.Rocks;

    public class AssemblyFactory
    {
        AssemblyDefinition assemblydefinition;
        readonly ModuleDefinition moduleDefinition;

        public AssemblyFactory(String name, ModuleDefinition moduleDefinition)
        {
            this.moduleDefinition = moduleDefinition;

            SetAssembly(name);
        }

        void SetAssembly(String name)
        {
            var assemblyReference = new AssemblyNameReference(name, Version.Parse("0.0.0.0"));
            assemblydefinition = moduleDefinition.AssemblyResolver.Resolve(assemblyReference);

            if (assemblydefinition is null)
                throw new ArgumentNullException($"Assembly not found");
        }

        TypeDefinition GetTypeDefinition(String fullname)
        {
            var typeDefinition = assemblydefinition.MainModule.GetTypes()
                .SingleOrDefault(_ => _.FullName == fullname);

            if (typeDefinition is null)
                throw new ArgumentNullException($"Type not found");

            return typeDefinition;
        }

        TypeReference GetTypeReference(TypeDefinition typeDefinition)
        {
            return moduleDefinition.ImportReference(typeDefinition);
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

            if (methodDefinition is null)
                throw new ArgumentNullException($"Method not found");

            var methodReference = typeReference.Module.ImportReference(methodDefinition);

            return methodReference;
        }
    }
}
