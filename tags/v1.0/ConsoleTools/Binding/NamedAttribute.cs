using System;

namespace ConsoleTools.Binding {
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
        /// True, если аргумент может быть представлен флагом, иначе false.
        /// </summary>
        public bool Switch { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута для разметки именованных аргументов.
        /// </summary>
        /// <param name="key">Ключ для связывания аргументов со свойствами. Может быть указан в формате &lt;name&gt;[;alias].</param>
        public NamedAttribute(string key) : base(key)
        {
            this.key = key;
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.Key = key;
            metadata.PropertyKind = Kind.Named;
            metadata.Meaning = key.Name;
            metadata.IsSwitch = Switch;
        }
    }
}