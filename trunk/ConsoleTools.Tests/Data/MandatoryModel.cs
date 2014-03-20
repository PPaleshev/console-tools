using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    /// <summary>
    /// ����� ��� �������� �������� ������������ � �������������� ����������.
    /// </summary>
    public class MandatoryModel
    {
        [Named("required;r", IsRequired = true)]
        public string RequiredValue { get; set; }

        [Named("optional;o", DefaultValue = "bazzinga")]
        public string OptionalValue { get; set; }
    }
}