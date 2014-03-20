using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using ConsoleTools.Conversion;

namespace ConsoleTools.Binding {
    /// <summary>
    /// Класс, представляющий собой метаданные, необходимые в процессе связывания значения.
    /// </summary>
    public class PropertyMetadata
    {
        /// <summary>
        /// Модель свойства.
        /// </summary>
        public PropertyInfo Property { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Тип свойства, определяющий правила её связывания.
        /// </summary>
        public Kind PropertyKind { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Назначение свойства.
        /// </summary>
        public string Meaning { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Флаг, равный true, если значение опции обязательно для указания, иначе false.
        /// </summary>
        public bool IsRequired { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Описание опции.
        /// </summary>
        public string Description { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Значение по умолчанию.
        /// </summary>
        public object DefaultValue { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Дескриптор свойства.
        /// </summary>
        public IConverter Converter { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Значения свойств, специфичных для аргументов.
        /// </summary>
        public PropertySpecification Specification { get; private set; }

        /// <summary>
        /// Создаёт новый объект метаданных свойства.
        /// </summary>
        /// <param name="property">Модель редактируемого свойства.</param>
        PropertyMetadata(PropertyInfo property)
        {
            Property = property;
            Specification = new PropertySpecification();
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Читает метаданные из свойства.
        /// </summary>
        /// <param name="property">Обрабатываемое свойство.</param>
        /// <param name="attr">Атрибут, управляющий связыванием.</param>
        public static PropertyMetadata ReadFromProperty(PropertyInfo property, ModelBindingAttribute attr)
        {
            var meta = new PropertyMetadata(property);
            meta.PropertyKind = attr.GetPropertyKind();
            meta.IsRequired = attr.IsRequired;
            meta.DefaultValue = attr.DefaultValue;
            meta.Meaning = attr.Meaning;
            meta.Description = attr.Description;
            meta.Converter = GetPropertyConverter(property);
            meta.Specification.IsCollection = typeof(IList).IsAssignableFrom(property.PropertyType);
            UpdateSpecifications(property, meta.Specification);
            return meta;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Возвращает конвертер, используемый для преобразования строковых значений в объекты типа свойства.
        /// </summary>
        static IConverter GetPropertyConverter(PropertyInfo property)
        {
            var attributes = Attribute.GetCustomAttributes(property);
            var provider = (IConverterProvider) attributes.FirstOrDefault(attribute => attribute is IConverterProvider);
            if (provider != null)
                return provider.GetConverter(property.PropertyType);
            var converter = (IConverter) attributes.FirstOrDefault(a => a is IConverter);
            return converter ?? Converters.GetConverterForType(property.PropertyType);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Читает дополнительные спецификации
        /// </summary>
        /// <param name="property">Модель свойства.</param>
        /// <param name="spec">Объект для установки специфичных свойств.</param>
        static void UpdateSpecifications(PropertyInfo property, PropertySpecification spec)
        {
            var attributes = (SpecificationAttribute[]) Attribute.GetCustomAttributes(property, typeof (SpecificationAttribute));
            foreach (SpecificationAttribute attribute in attributes)
                attribute.UpdateSpecification(spec);
        }
    }
}
