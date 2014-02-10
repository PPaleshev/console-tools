using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// Базовый атрибут для всех атрибутов, контролирующих способ связывания аргументов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ModelBindingAttribute : Attribute
    {
        /// <summary>
        /// Название обрамляемого свойства.
        /// </summary>
        readonly string meaning;

        /// <summary>
        /// Флаг, равный true, если значение свойства должно быть обязательно указано, иначе false.
        /// По умолчанию равен false.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Название обрамляемого свойства.
        /// </summary>
        public string Meaning
        {
            get { return meaning; }
        }

        /// <summary>
        /// Создаёт новый экземпляр атрибута.
        /// </summary>
        /// <param name="meaning">Название обрамляемого свойства.</param>
        protected ModelBindingAttribute(string meaning)
        {
            this.meaning = meaning;
        }

        /// <summary>
        /// Реализованный в потомках, заполняет метаданные специфичными параметрами.
        /// </summary>
        /// <param name="metadata">Метаданные свойства.</param>
        public abstract void FillMetadata(PropertyMetadata metadata);
    }
}