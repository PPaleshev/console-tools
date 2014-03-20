using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            return Attribute.GetCustomAttribute(modelType, typeof (NamedArgumentsPolicyAttribute)) as NamedArgumentsPolicyAttribute;
        }

        /// <summary>
        /// Читает список метаданных свойств модели.
        /// </summary>
        /// <param name="modelType">Тип модели.</param>
        /// <returns>Возвращает список метаданных свойств модели.</returns>
        public static IList<PropertyMetadata> ReadPropertyMetadata(Type modelType)
        {
            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var data = from property in properties
                let attr = (ModelBindingAttribute) Attribute.GetCustomAttribute(property, typeof (ModelBindingAttribute), false)
                where attr != null
                select PropertyMetadata.ReadFromProperty(property, attr);
            return data.ToList();
        }
    }
}