using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Вспомогательный класс для чтения метаданных модели.
    /// </summary>
    static class MetadataProvider
    {
        /// <summary>
        /// Возвращает атрибут, содержащий настройки для разбора именованных аргументов.
        /// </summary>
        /// <param name="modelType">Тип модели.</param>
        /// <returns>Если модель не содержит указанного атрибута, возвращает <c>null</c>.</returns>
        public static NamedArgumentsPolicyAttribute GetNamedArgumentsPolicy(Type modelType)
        {
            return Attribute.GetCustomAttribute(modelType, typeof(NamedArgumentsPolicyAttribute)) as NamedArgumentsPolicyAttribute;
        }

        /// <summary>
        /// Читает список метаданных свойств модели.
        /// </summary>
        /// <param name="modelType">Тип модели.</param>
        /// <returns>Возвращает список метаданных свойств модели.</returns>
        public static IList<PropertyMetadata> ReadPropertyMetadata(Type modelType)
        {
            var descriptors = TypeDescriptor.GetProperties(modelType);
            var propertyMetadata = new List<PropertyMetadata>();
            foreach (PropertyDescriptor property in descriptors)
            {
                var attr = (ModelBindingAttribute)property.Attributes[typeof(ModelBindingAttribute)];
                if (attr == null)
                    continue;
                var metadata = new PropertyMetadata(property);
                metadata.IsRequired = attr.IsRequired;
                metadata.IsSwitch = property.Attributes[typeof(SwitchAttribute)] != null;
                attr.FillMetadata(metadata);
                propertyMetadata.Add(metadata);
            }
            return propertyMetadata;
        }
    }
}