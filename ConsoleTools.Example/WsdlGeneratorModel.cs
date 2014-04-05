using ConsoleTools.Binding;
using ConsoleTools.Conversion;

namespace ConsoleTools.Example
{
    /// <summary>
    /// Модель входных аргументов для генератора сборок по wsdl.
    /// </summary>
    [NamedArgumentsPolicy("/",':')]
    public class WsdlGeneratorModel
    {
        /// <summary>
        /// Пространство имён, в котором должна быть создана сборка.
        /// </summary>
        [Named("namespace;ns", DefaultValue = "SomeAssembly.Data", Description = "Namespace containing generated classes")]
        public string Namespace { get; set; }

        /// <summary>
        /// URL места, где расположен файл с WSDL сервиса.
        /// </summary>
        [Named("wsdl", IsRequired = true, Description = "URL which locates source wsdl file to generate assembly.")]
        public string WsdlUrl { get; set; }

        /// <summary>
        /// Флаг, равный true, если <see cref="WsdlUrl"/> указывает на удалённый источник, иначе на локальный.
        /// </summary>
        [Named("remote",IsSwitch = true)]
        public bool IsRemote { get; set; }

        /// <summary>
        /// Версия сборки.
        /// </summary>
        [Named("version;v", IsRequired = false, DefaultValue = "1.0", Description = "Assembly version in <Major>.<Minor>.<Build>.<Revision> format.")]
        public string Version { get; set; }

        /// <summary>
        /// Название сборки с результатом.
        /// </summary>
        [Named("out", DefaultValue = "Data.dll", Description = "Output assembly file name (including extension or path).")]
        public string AssemblyFileName { get; set; }

        /// <summary>
        /// Массив дополнительных импортов.
        /// </summary>
        [Named("usings", Description = "Collection of used additional namespaces.")]
        [CollectionConversion(";")]
        public string[] ExtraUsings { get; set; }
    }
}