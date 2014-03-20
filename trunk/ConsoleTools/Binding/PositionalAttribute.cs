using System;

namespace ConsoleTools.Binding
{
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

        public override Kind GetPropertyKind()
        {
            return Kind.Positional;
        }

        public override void UpdateSpecification(PropertySpecification spec)
        {
            spec.Position = Position;
        }
    }
}