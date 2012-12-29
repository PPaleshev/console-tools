using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;


namespace ConsoleTools.Conversion {
    public class ValueListConverter : TypeConverter {
        #region Data

        private const string DefaultListItemSeparator = ",";

        #endregion

        #region Overrides

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (typeof (string) == sourceType) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        //----------------------------------------------------------------------[]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            string str = (string) value;
            string[] items = string.IsNullOrEmpty(str)
                                 ? new string[0]
                                 : ParsePropertyValue(context, str);

            ArrayList convertedItems = ConvertItems(context, items);
            return FormatResult(context, convertedItems);
        }

        #endregion

        #region Routines

        private static string[] ParsePropertyValue(ITypeDescriptorContext context, string value) {
            string listItemSeparator = GetListItemSeparator(context);
            return value.Split(new string[] {listItemSeparator}, StringSplitOptions.RemoveEmptyEntries);
        }

        //----------------------------------------------------------------------[]
        private static string GetListItemSeparator(ITypeDescriptorContext context) {
            ListItemSeparatorAttribute a = context.PropertyDescriptor.Attributes[typeof (ListItemSeparatorAttribute)]
                                           as ListItemSeparatorAttribute;
            if (a == null || string.IsNullOrEmpty(a.Separator)) {
                return DefaultListItemSeparator;
            }
            return a.Separator;
        }

        //----------------------------------------------------------------------[]
        private static Type GetItemType(ITypeDescriptorContext context) {
            Type propertyType = context.PropertyDescriptor.PropertyType;
            if (propertyType.IsArray) {
                return propertyType.GetElementType();
            }

            Type genericType = propertyType.GetGenericTypeDefinition();
            if (genericType != null && propertyType.GetGenericArguments().Length == 1) {
                return propertyType.GetGenericArguments()[0];
            }

            return typeof (string);
        }

        //----------------------------------------------------------------------[]
        private static ArrayList ConvertItems(ITypeDescriptorContext context, string[] items) {
            Type itemType = GetItemType(context);
            TypeConverter converter = TypeDescriptor.GetConverter(itemType);

            ArrayList result = new ArrayList(items.Length);
            for (int i = 0; i < items.Length; i++) {
                result.Add(converter.CanConvertFrom(context, typeof (string))
                               ? converter.ConvertFromString(items[i])
                               : items[i]);
            }
            return result;
        }

        //----------------------------------------------------------------------[]
        private static object FormatResult(ITypeDescriptorContext context, ArrayList result) {
            Type propertyType = context.PropertyDescriptor.PropertyType;
            if (propertyType.IsArray) {
                return result.ToArray(propertyType.GetElementType());
            }

            IList collection = (IList) Activator.CreateInstance(propertyType);
            foreach (object o in result) {
                collection.Add(o);
            }
            return collection;
        }

        #endregion
    }
}