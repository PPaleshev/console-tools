using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;


namespace ConsoleTools.Conversion {
    /// <summary>
    /// Реализация <see cref="TypeConverter"/> для преобразования списковых значений с учётом разделителей.
    /// </summary>
    public class CollectionArgumentConverter : TypeConverter
    {
        /// <summary>
        /// Разделитель элементов списка, используемый по умолчанию.
        /// </summary>
        public const string DefaultListItemSeparator = ",";

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return typeof(string) == sourceType || base.CanConvertFrom(context, sourceType);
        }

        //----------------------------------------------------------------------[]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            var str = (string) value;
            var items = string.IsNullOrEmpty(str) ? new string[0] : ParsePropertyValue(context, str);
            var convertedItems = ConvertItems(context, items);
            return CollectResult(context, convertedItems);
        }

        /// <summary>
        /// Разбирает значение свойства, предположительно содержащее список значений.
        /// </summary>
        /// <param name="context">Контекст описания типа.</param>
        /// <param name="value">Значение для разбора.</param>
        /// <returns>Возвращает массив элементов, содержащихся в исходном значении.</returns>
        private static string[] ParsePropertyValue(ITypeDescriptorContext context, string value) {
            var listItemSeparator = GetListItemSeparator(context);
            return value.Split(new[] {listItemSeparator}, StringSplitOptions.RemoveEmptyEntries);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Возвращает разделитель элементов коллекции.
        /// </summary>
        /// <param name="context">Контекст описания типа.</param>
        static string GetListItemSeparator(ITypeDescriptorContext context)
        {
            var a = context.PropertyDescriptor.Attributes[typeof(CollectionItemSeparatorAttribute)] as CollectionItemSeparatorAttribute;
            return a == null || string.IsNullOrEmpty(a.Separator) ? DefaultListItemSeparator : a.Separator;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Возвращает тип элемента коллекции.
        /// </summary>
        /// <param name="context">Контекст описания типа.</param>
        private static Type GetCollectionItemType(ITypeDescriptorContext context) {
            var propertyType = context.PropertyDescriptor.PropertyType;
            if (propertyType.IsArray)
                return propertyType.GetElementType();

            var genericType = propertyType.GetGenericTypeDefinition();
            if (genericType != null && propertyType.GetGenericArguments().Length == 1)
                return propertyType.GetGenericArguments()[0];
            return typeof (string);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Выполняет преобразования строковых элементов списка в их реальный тип, определяемый свойством.
        /// </summary>
        /// <param name="context">Контекст преобразования.</param>
        /// <param name="items">Список элементов коллекции.</param>
        /// <returns>Возвращает список с реальными значениями результирующей коллекции.</returns>
        private static ArrayList ConvertItems(ITypeDescriptorContext context, string[] items) {
            var itemType = GetCollectionItemType(context);
            var converter = TypeDescriptor.GetConverter(itemType);

            var result = new ArrayList(items.Length);
            foreach (string t in items)
                result.Add(converter.CanConvertFrom(context, typeof(string)) ? converter.ConvertFromString(t) : t);
            return result;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Собирает элементы <paramref name="items"/> в массив или другой тип коллекции в зависимости от типа свойства.
        /// </summary>
        /// <param name="context">Контекст преобразования свойства.</param>
        /// <param name="items">Элементы списка.</param>
        /// <returns>Возвращает соответствующую коллекцию, содержащую элементы списка.</returns>
        private static object CollectResult(ITypeDescriptorContext context, ArrayList items)
        {
            var propertyType = context.PropertyDescriptor.PropertyType;
            if (propertyType.IsArray)
                return items.ToArray(propertyType.GetElementType());

            var collection = (IList)Activator.CreateInstance(propertyType);
            foreach (var o in items)
                collection.Add(o);
            return collection;
        }
    }
}