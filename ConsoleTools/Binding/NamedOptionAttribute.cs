using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// �������, ������� ���������� �������� �������, ������� ������ ���� ������� � ������������ �����������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NamedOptionAttribute : OptionBindingAttribute
    {
        /// <summary>
        /// ���� ���������, �� �������� ����������� ����������.
        /// </summary>
        readonly OptionKey key;

        /// <summary>
        /// ������ ����� ��������� �������� ��� �������� ����������� ����������.
        /// </summary>
        /// <param name="key">���� ��� ���������� ���������� �� ����������. ����� ���� ������ � ������� &lt;name&gt;[;alias].</param>
        /// <param name="isRequired">����, ������ true, ���� �������� ����������� ��� ����������, ����� false.</param>
        public NamedOptionAttribute(string key, bool isRequired = false)
            : base(isRequired)
        {
            this.key = key;
        }

        public override void FillMetadata(OptionMetadata metadata)
        {
            metadata.Key = key;
            metadata.OptionType = OptionType.Named;
        }
    }
}