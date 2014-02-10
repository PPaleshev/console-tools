using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// �������, ������� ���������� ��������, � ������� ����� ����������� ������ ����������, ������� �� ������� ������� � �������� ��������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnboundAttribute : ModelBindingAttribute
    {
        /// <summary>
        /// ������ ����� ��������� ��������.
        /// </summary>
        /// <param name="meaning">�������� �������� ��� ������������. ���� �������� �� ������, �� ��� �� ����� ������������ ������������ ��� ������ �������� �������������.</param>
        public UnboundAttribute(string meaning = "")
            : base(meaning)
        {
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.PropertyKind = Kind.Unbound;
            metadata.Meaning = Meaning;
        }
    }
}