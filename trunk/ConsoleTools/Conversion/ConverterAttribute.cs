using System;
using System.Globalization;

namespace ConsoleTools.Conversion
{
    /// <summary>
    /// Абстрактный атрибут, посредством которого задаётся способ преобразования значений обрамляемого свойства из строк.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ConverterAttribute : Attribute, IConverter
    {
        public abstract bool CanConvertTo(Type targetType);

        public abstract object ConvertFromString(string value, CultureInfo culture, Type targetType);
    }
}