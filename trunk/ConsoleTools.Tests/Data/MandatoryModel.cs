using System.ComponentModel;
using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    /// <summary>
    /// Класс для проверки привязки обязательных и необязательных параметров.
    /// </summary>
    public class MandatoryModel
    {
        [Named("required;r", Required = true)]
        public string RequiredValue { get; set; }

        [Named("optional;o"), DefaultValue("bazzinga")]
        public string OptionalValue { get; set; }
    }
}