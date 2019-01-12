namespace Kasay.DependencyProperty.WPF.Fody
{
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using System;
    using System.Linq;

    internal class ConstructorImplementer
    {
        readonly AssemblyFactory presentationAssembly;
        readonly TypeDefinition typeDefinition;

        ModuleDefinition moduleDefinition;

        public ConstructorImplementer(
            AssemblyFactory presentationAssembly, 
            TypeDefinition typeDefinition,
            Boolean isModeTest)
        {
            this.presentationAssembly = presentationAssembly;
            this.typeDefinition = typeDefinition;

            moduleDefinition = typeDefinition.Module;

            if (!isModeTest)
                EqualDataContext();

            AddStaticConstructor();
        }

        void EqualDataContext()
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
}
