using System.ComponentModel;
using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    /// <summary>
    /// Класс для проверки привязки обязательных и необязательных параметров.
    /// </summary>
    public class MandatoryOptions
    {
        [NamedOption("required;r", true)]
        public string RequiredValue { get; set; }

        [NamedOption("optional;o"), DefaultValue("bazzinga")]
        public string OptionalValue { get; set; }
    }
}