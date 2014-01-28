using System;


namespace ConsoleTools.Conversion {
    /// <summary>
    /// Атрибут для указания разделителя элементов коллекции.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionItemSeparatorAttribute : Attribute
    {
        /// <summary>
        /// Разделитель элементов коллекции, используемый при преобразовании списочных аргументов.
        /// </summary>
        public string Separator { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута с указанием значения разделителя.
        /// </summary>
        /// <param name="separator">Разделитель элементов коллекции.</param>
        public CollectionItemSeparatorAttribute(string separator)
        {
            Separator = string.IsNullOrEmpty(separator) ? "," : separator;
        }
    }
}