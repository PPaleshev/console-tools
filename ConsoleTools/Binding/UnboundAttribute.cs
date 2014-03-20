using System;
using System.Collections;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// �������, ������� ���������� ��������, � ������� ����� ����������� ������ ����������, ������� �� ������� ������� � �������� ��������.
    /// ��� ��������, ����������� ������ ���������, ������ ���� ����������, ����������� <see cref="IList"/>. ��� ���������� �������� �� ����������� ��������������
    /// ����� ���������, ������� ��� ��������� ������ ���� ���� <see cref="object"/> ���� <see cref="string"/>.
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

        public override Kind GetPropertyKind()
        {
            return Kind.Unbound;
        }

        public override void UpdateSpecification(PropertySpecification spec)
        {
        }
    }
}