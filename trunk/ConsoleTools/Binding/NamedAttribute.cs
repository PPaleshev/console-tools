using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, которым помечаются свойства объекты, которые должны быть связаны с именованными аргументами.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NamedAttribute : ModelBindingAttribute
    {
        /// <summary>
        /// Ключ аргумента, по которому выполняется связывание.
        /// </summary>
        readonly PropertyKey key;

        /// <summary>
        /// True, если аргумент представлен флагом, иначе false.
        /// </summary>
        public bool IsSwitch { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута для разметки именованных аргументов.
        /// </summary>
        /// <param name="key">Ключ для связывания аргументов со свойствами. Может быть указан в формате &lt;name&gt;[;alias].</param>
        public NamedAttribute(string key) : base(key)
        {
            this.key = key;
        }

        public override void UpdateSpecification(PropertySpecification spec)
        {
            spec.IsSwitch = IsSwitch;
            spec.Key = key;
        }

        public override Kind GetPropertyKind()
        {
            return Kind.Named;
        }
    }
}