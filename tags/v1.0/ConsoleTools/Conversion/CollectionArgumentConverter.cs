using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;


namespace ConsoleTools.Conversion {
    /// <summary>
    /// ���������� <see cref="TypeConverter"/> ��� �������������� ��������� �������� � ������ ������������.
    /// </summary>
    public class CollectionArgumentConverter : TypeConverter
    {
        /// <summary>
        /// ����������� ��������� ������, ������������ �� ���������.
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
        /// ��������� �������� ��������, ���������������� ���������� ������ ��������.
        /// </summary>
        /// <param name="context">�������� �������� ����.</param>
        /// <param name="value">�������� ��� �������.</param>
        /// <returns>���������� ������ ���������, ������������ � �������� ��������.</returns>
        private static string[] ParsePropertyValue(ITypeDescriptorContext context, string value) {
            var listItemSeparator = GetListItemSeparator(context);
            return value.Split(new[] {listItemSeparator}, StringSplitOptions.RemoveEmptyEntries);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ����������� ��������� ���������.
        /// </summary>
        /// <param name="context">�������� �������� ����.</param>
        static string GetListItemSeparator(ITypeDescriptorContext context)
        {
            var a = context.PropertyDescriptor.Attributes[typeof(CollectionItemSeparatorAttribute)] as CollectionItemSeparatorAttribute;
            return a == null || string.IsNullOrEmpty(a.Separator) ? DefaultListItemSeparator : a.Separator;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ��� �������� ���������.
        /// </summary>
        /// <param name="context">�������� �������� ����.</param>
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
        /// ��������� �������������� ��������� ��������� ������ � �� �������� ���, ������������ ���������.
        /// </summary>
        /// <param name="context">�������� ��������������.</param>
        /// <param name="items">������ ��������� ���������.</param>
        /// <returns>���������� ������ � ��������� ���������� �������������� ���������.</returns>
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
        /// �������� �������� <paramref name="items"/> � ������ ��� ������ ��� ��������� � ����������� �� ���� ��������.
        /// </summary>
        /// <param name="context">�������� �������������� ��������.</param>
        /// <param name="items">�������� ������.</param>
        /// <returns>���������� ��������������� ���������, ���������� �������� ������.</returns>
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