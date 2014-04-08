using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, которым могут быть отмечены части модели, тип которых должен вычисляться во время исполнения.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PartAttribute : Attribute
    {
        /// <summary>
        /// Порядок вычисления динамической части. По умолчанию равен нулю.
        /// Может использоваться для моделей с несколькими динамическими частями для задания порядка вычисления.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Флаг, равный true, если тип свойства, отмечеенного атрибутом, должен вычисляться на основании значения свойства, иначе на основании типа свойства
        /// </summary>
        public bool IsDynamic { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута.
        /// </summary>
        /// <param name="isDynamic">Флаг, равный true, если тип свойства, отмечеенного атрибутом, должен вычисляться на основании значения свойства, иначе на основании типа свойства.</param>
        /// <param name="order">Порядок вычисления свойств части.</param>
        public PartAttribute(bool isDynamic = false, int order = 0)
        {
            Order = order;
            IsDynamic = isDynamic;
        }
    }
}