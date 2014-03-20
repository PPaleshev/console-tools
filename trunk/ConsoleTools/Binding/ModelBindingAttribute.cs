using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// Базовый атрибут для всех атрибутов, контролирующих способ связывания аргументов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ModelBindingAttribute : SpecificationAttribute
    {
        /// <summary>
        /// Смысловое значение обрамляемого свойства.
        /// </summary>
        readonly string meaning;

        /// <summary>
        /// Флаг, равный true, если значение свойства должно быть обязательно указано, иначе false.
        /// По умолчанию равен false.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Значение по умолчанию. Используется только если свойство является необязательным.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Описание свойства.
        /// </summary>
        public string Description { get; set; }

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
        /// Возвращает тип аргумента, описываемого атрибутом.
        /// </summary>
        public abstract Kind GetPropertyKind();
    }
}