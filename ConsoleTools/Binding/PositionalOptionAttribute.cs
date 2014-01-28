using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// Атрибут для разметки позиционных аргументов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PositionalOptionAttribute : OptionBindingAttribute
    {
        /// <summary>
        /// Номер привязываемого атрибута.
        /// Атрибуты нумеруются, начиная с 0.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="isRequired"></param>
        public PositionalOptionAttribute(int position, bool isRequired = false)
            : base(isRequired)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");
            Position = position;
        }

        public override void FillMetadata(OptionMetadata metadata)
        {
            metadata.OptionType = OptionType.Positional;
            metadata.Position = Position;
        }
    }
}