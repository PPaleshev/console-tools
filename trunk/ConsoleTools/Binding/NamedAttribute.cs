using System;

namespace ConsoleTools.Binding
{
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
        /// True, ���� �������� ����������� ������, ����� false.
        /// </summary>
        public bool IsSwitch { get; set; }

        /// <summary>
        /// ������ ����� ��������� �������� ��� �������� ����������� ����������.
        /// </summary>
        /// <param name="key">���� ��� ���������� ���������� �� ����������. ����� ���� ������ � ������� &lt;name&gt;[;alias].</param>
        public NamedAttribute(string key) : base(key)
        {
            this.key = key;
        }

        public override void UpdateSpecification(PropertySpecification spec)
        {
            spec.IsSwitch = IsSwitch;
            spec.Key = key;
        }

        public override Kind GetPropertyKind()
        {
            return Kind.Named;
        }
    }
}