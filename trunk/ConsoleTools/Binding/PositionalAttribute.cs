using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут для разметки позиционных аргументов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PositionalAttribute : ModelBindingAttribute
    {
        /// <summary>
        /// Номер привязываемого атрибута.
        /// Атрибуты нумеруются, начиная с 0.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута.
        /// </summary>
        /// <param name="meaning">Назначение обрамляемого свойства.</param>
        /// <param name="position">Позиция аргумента.</param>
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