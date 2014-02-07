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
        public UnboundAttribute()
            : base(false)
        {
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.PropertyKind = Kind.Unbound;
        }
    }
}