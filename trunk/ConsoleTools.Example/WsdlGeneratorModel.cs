using ConsoleTools.Binding;

namespace ConsoleTools.Example
{
    /// <summary>
    /// Модель входных аргументов для генератора сборок по wsdl.
    /// </summary>
    public class WsdlGeneratorModel
    {
        /// <summary>
        /// URL места, где расположен файл с WSDL сервиса.
        /// </summary>
        [Named("wsdl", IsRequired = true, Description = "URL which locates source wsdl to generate assembly.")]
        public string WsdlUrl { get; set; }

        /// <summary>
        /// Пространство имён, в котором должна быть создана сборка.
        /// </summary>
        [Named("namespace;ns", DefaultValue = "SomeAssembly.Data", Description = "Namespace containing generated classes")]
        public string RootNamespace { get; set; }

        /// <summary>
        /// Название сборки с результатом.
        /// </summary>
        [Named("assembly;asm", DefaultValue = "Data.dll", Description = "Output assembly file name (including extension).")]
        public string AssemblyName { get; set; }

        /// <summary>
        /// Версия сборки.
        /// </summary>
        [Named("version;v", IsRequired = true, Description = "Assembly version in <Major>.<Minor>.<Build>.<Revision> format.")]
        public string Version { get; set; }
    }
}