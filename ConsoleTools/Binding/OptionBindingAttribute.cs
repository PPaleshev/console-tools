using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// Базовый атрибут для всех атрибутов, контролирующих способ связывания аргументов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class OptionBindingAttribute : Attribute
    {
        /// <summary>
        /// Флаг, равный true, если значение обязательно для заполнения.
        /// </summary>
        readonly bool isRequired;

        /// <summary>
        /// Значение по умолчанию.
        /// </summary>
        object defaultValue;

        /// <summary>
        /// Флаг, равный true, если значение свойства должно быть обязательно указано, иначе false.
        /// </summary>
        public bool IsRequired
        {
            get { return isRequired; }
        }


        /// <summary>
        /// Создаёт новый экземпляр атрибута.
        /// </summary>
        /// <param name="isRequired">Флаг, равный true, если значение свойства должно быть обязательно указано, иначе false.</param>
        protected OptionBindingAttribute(bool isRequired)
        {
            this.isRequired = isRequired;
        }

        /// <summary>
        /// Реализованный в потомках, заполняет метаданные специфичными параметрами.
        /// </summary>
        /// <param name="metadata">Метаданные свойства.</param>
        public abstract void FillMetadata(OptionMetadata metadata);
    }
}