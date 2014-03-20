using System;
using System.Collections;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, которым помечается свойство, в котором будет содержаться список аргументов, которые не удалось связать в процессе привязки.
    /// Тип свойства, отмеченного данным атрибутом, должен быть коллекцией, реализующей <see cref="IList"/>. При связывании свойства не выполняется преобразований
    /// типов элементов, поэтому тип коллекции должен быть либо <see cref="object"/> либо <see cref="string"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnboundAttribute : ModelBindingAttribute
    {
        /// <summary>
        /// Создаёт новый экземпляр атрибута.
        /// </summary>
        /// <param name="meaning">Значение атрибута для пользователя. Если значение не задано, то оно не будет отображаться пользователю при выводе примеров использования.</param>
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