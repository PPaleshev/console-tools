using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// �������, ������� ���������� �������� �������, ������� ������ ���� ������� � ������������ �����������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NamedAttribute : ModelBindingAttribute
    {
        /// <summary>
        /// ���� ���������, �� �������� ����������� ����������.
        /// </summary>
        readonly PropertyKey key;

        /// <summary>
        /// ������ ����� ��������� �������� ��� �������� ����������� ����������.
        /// </summary>
        /// <param name="key">���� ��� ���������� ���������� �� ����������. ����� ���� ������ � ������� &lt;name&gt;[;alias].</param>
        /// <param name="isRequired">����, ������ true, ���� �������� ����������� ��� ����������, ����� false.</param>
        public NamedAttribute(string key, bool isRequired = false)
            : base(isRequired)
        {
            this.key = key;
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.Key = key;
            metadata.PropertyKind = Kind.Named;
        }
    }
}