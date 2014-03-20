using System;
using ConsoleTools.Binding;

namespace ConsoleTools.Conversion
{
    /// <summary>
    /// Атрибут для разметки аргументов, содержащих коллекции элементов, для которых необходимо применять нестандартные правила преобразования.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionConversionAttribute : SpecificationAttribute, IConverterProvider
    {
        /// <summary>
        /// Разделитель элементов коллекции при преобразовании из строки.
        /// </summary>
        readonly string elementSeparator;

        /// <summary>
        /// Тип конвертера, используемого для преобразования элементов коллекции.
        /// </summary>
        public Type ElementConverterType { get; set; }

        /// <summary>
        /// Формат для преобразования элементов коллекции. Необязателен для заполнения.
        /// Игнорируется для конвертеров, не поддерживающих строки форматирования.
        /// </summary>
        public string ElementFormatString { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр конвертера с указанием разделителя элементов, конвертера и формата разбора элементов коллекции.
        /// </summary>
        /// <param name="elementSeparator">Разделитель элементов в строке аргументов.</param>
        public CollectionConversionAttribute(string elementSeparator)
        {
            if (string.IsNullOrEmpty(elementSeparator))
                throw new ArgumentNullException("elementSeparator");
            this.elementSeparator = elementSeparator;
        }

        public IConverter GetConverter(Type type)
        {
            if (!CollectionConverter.CanConvertFrom(type))
                throw new InvalidOperationException(string.Format("{0} can convert only collections that implements IList", typeof (CollectionConverter).Name));
            if (ElementConverterType == null)
                return new CollectionConverter(elementSeparator, Converters.GetConverterForType(CollectionConverter.GetElementType(type), ElementFormatString));
            if (!typeof (IConverter).IsAssignableFrom(ElementConverterType))
                throw new InvalidOperationException(string.Format("'{0}' is not valid converter type (does not implement {1})", ElementConverterType.Name, typeof (IConverter).Name));
            return CreateConverter();
        }

        public override void UpdateSpecification(PropertySpecification spec)
        {
            spec.CollectionItemSeparator = string.IsNullOrEmpty(elementSeparator) ? CollectionConverter.DEFAULT_SEPARATOR : elementSeparator;
            spec.Format = ElementFormatString;
        }

        /// <summary>
        /// Создаёт экземпляр конвертера.
        /// Если тип конвертера содержит конструктор со строковым параметром, то вызывается он, передавая в качестве аргумента формат элемента.
        /// Если тип конвертера содержит конструктор по умолчанию, то вызывается он.
        /// Если не найдены вышеперечисленные конструкторы, бросается исключение.
        /// </summary>
        IConverter CreateConverter()
        {
            var ctor = ElementConverterType.GetConstructor(new[] {typeof (string)});
            if (ctor != null)
                return (IConverter) ctor.Invoke(new object[] {ElementFormatString});
            ctor = ElementConverterType.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
                return (IConverter) ctor.Invoke(new object[0]);
            throw new InvalidOperationException("Failed to create converter of type '{0}' because it doesn't contain default constructor or constructor with format argument");
        }
    }
}