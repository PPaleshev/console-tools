using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// �������, ������� ���������� ��������, � ������� ����� ����������� ������ ����������, ������� �� ������� ������� � �������� ��������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnboundOptionsAttribute : OptionBindingAttribute
    {
        /// <summary>
        /// ������ ����� ��������� ��������.
        /// </summary>
        public UnboundOptionsAttribute()
            : base(false)
        {
        }

        public override void FillMetadata(OptionMetadata metadata)
        {
            metadata.OptionType = OptionType.Unbound;
        }
    }
}