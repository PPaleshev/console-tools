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
        /// True, ���� �������� ����� ���� ����������� ������, ����� false.
        /// </summary>
        public bool Switch { get; set; }

        /// <summary>
        /// ������ ����� ��������� �������� ��� �������� ����������� ����������.
        /// </summary>
        /// <param name="key">���� ��� ���������� ���������� �� ����������. ����� ���� ������ � ������� &lt;name&gt;[;alias].</param>
        public NamedAttribute(string key) : base(key)
        {
            this.key = key;
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.Key = key;
            metadata.PropertyKind = Kind.Named;
            metadata.Meaning = key.Name;
            metadata.IsSwitch = Switch;
        }
    }
}