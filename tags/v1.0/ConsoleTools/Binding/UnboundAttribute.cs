using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, которым помечается свойство, в котором будет содержаться список аргументов, которые не удалось связать в процессе привязки.
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

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.PropertyKind = Kind.Unbound;
            metadata.Meaning = Meaning;
        }
    }
}