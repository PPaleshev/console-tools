using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, позволяющий специфицировать метаданные свойства.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class SpecificationAttribute : Attribute
    {
        /// <summary>
        /// Заполняет специфичные значения свойств.
        /// </summary>
        /// <param name="spec">Спецификация метаданных.</param>
        public abstract void UpdateSpecification(PropertySpecification spec);
    }
}