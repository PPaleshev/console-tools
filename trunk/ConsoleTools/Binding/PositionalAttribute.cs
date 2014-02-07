using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// ������� ��� �������� ����������� ����������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PositionalAttribute : ModelBindingAttribute
    {
        /// <summary>
        /// ����� �������������� ��������.
        /// �������� ����������, ������� � 0.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// ������ ����� ��������� ��������.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="isRequired"></param>
        public PositionalAttribute(int position, bool isRequired = false)
            : base(isRequired)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");
            Position = position;
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.PropertyKind = Kind.Positional;
            metadata.Position = Position;
        }
    }
}