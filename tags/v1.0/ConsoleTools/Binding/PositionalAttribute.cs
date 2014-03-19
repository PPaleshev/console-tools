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
        /// <param name="meaning">���������� ������������ ��������.</param>
        /// <param name="position">������� ���������.</param>
        public PositionalAttribute(string meaning, int position)
            : base(meaning)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");
            Position = position;
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.PropertyKind = Kind.Positional;
            metadata.Position = Position;
            metadata.Meaning = Meaning;
        }
    }
}