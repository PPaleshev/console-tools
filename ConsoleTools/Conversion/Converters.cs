using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleTools.Conversion
{
    /// <summary>
    /// Фасад для доступа к известным конвертерам.
    /// </summary>
    static class Converters
    {
        /// <summary>
        /// Возвращает один из предустановленных конвертеров для преобразования значений типа <paramref name="type"/> из строк.
        /// </summary>
        /// <param name="type">Тип значения для преобразования.</param>
        /// <param name="format">Используемый формат преобразования. Если не задан, используется формат по умолчанию.</param>
        public static IConverter GetConverterForType(Type type, string format = "")
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (BooleanConverter.CanConvertTo(type))
                return new BooleanConverter();
            if (TransparentConverter.CanConvertTo(type))
                return TransparentConverter.Instance;
            if (NumberConverter.CanConvertTo(type))
                return new NumberConverter();
            if (DecimalConverter.CanConvertTo(type))
                return new DecimalConverter();
            if (TimeSpanConverter.CanConvertTo(type))
                return new TimeSpanConverter(format);
            if (DateTimeConverter.CanConvertTo(type))
                return new DateTimeConverter(format);
            if (EnumConverter.CanConvertTo(type))
                return new EnumConverter();
            if (CollectionConverter.CanConvertFrom(type))
                return new CollectionConverter(GetConverterForType(CollectionConverter.GetElementType(type), format));
            throw new InvalidOperationException("failed to find suitable converter for type " + type);
        }
    }

    /// <summary>
    /// Базовый конвертер, используемый для преобразования <see cref="ValueType"/> и их <see cref="Nullable{T}"/> представлений.
    /// </summary>
    public abstract class BaseNullableConverter : IConverter
    {
        public object ConvertFromString(string value, CultureInfo culture, Type targetType)
        {
            var actualType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (string.IsNullOrWhiteSpace(value) && actualType != targetType)
                return null;
            return ConvertCore(value, culture, actualType);
        }

        /// <summary>
        /// Возвращает преобразованное значение.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        /// <param name="culture">Используемая культура.</param>
        /// <param name="targetType">Тип для преобразования. Всегда содержит актуальный тип (не <see cref="Nullable{T}"/>).</param>
        protected abstract object ConvertCore(string value, CultureInfo culture, Type targetType);
    }

    /// <summary>
    /// Конвертер для преобразования числовых типов данных.
    /// </summary>
    public class NumberConverter : BaseNullableConverter
    {
        /// <summary>
        /// Множество поддерживаемых типов.
        /// </summary>
        internal static readonly HashSet<Type> SUPPORTED_TYPES = new HashSet<Type>
        {
            typeof (byte), typeof(sbyte),
            typeof (short), typeof (ushort),
            typeof (int), typeof(uint), 
            typeof (long), typeof (ulong),
            typeof(float),
            typeof(double)
        };

        /// <summary>
        /// Возвращает true, если текущий конвертер может выполнять преобразования в указанный тип данных.
        /// </summary>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertTo(Type type)
        {
            return SUPPORTED_TYPES.Contains(type);
        }

        protected override object ConvertCore(string value, CultureInfo culture, Type targetType)
        {
            return Convert.ChangeType(value, targetType, culture);
        }
    }

    /// <summary>
    /// Конвертер в тип данных <see cref="decimal"/>.
    /// </summary>
    public class DecimalConverter : BaseNullableConverter
    {
        /// <summary>
        /// Возвращает true, если текущий конвертер может выполнять преобразования в указанный тип данных.
        /// </summary>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertTo(Type type)
        {
            return typeof(decimal) == type;
        }

        protected override object ConvertCore(string value, CultureInfo culture, Type targetType)
        {
            return decimal.Parse(value, NumberStyles.Float, culture);
        }
    }

    /// <summary>
    /// Конвертер для значений типа <see cref="DateTime"/>.
    /// </summary>
    public class DateTimeConverter : BaseNullableConverter
    {
        /// <summary>
        /// Экземпляр общего конвертера.
        /// </summary>
        public static readonly DateTimeConverter Instance = new DateTimeConverter();
        
        /// <summary>
        /// Формат даты-времени по умолчанию.
        /// </summary>
        readonly string format;

        /// <summary>
        /// Создаёт новый экземпляр конвертера для объектов типа <see cref="DateTime"/> с указанием формата.
        /// </summary>
        /// <param name="format">Формат преобразования значений.</param>
        public DateTimeConverter(string format = "")
        {
            this.format = format;
        }

        /// <summary>
        /// Возвращает true, если текущий конвертер может выполнять преобразования в указанный тип данных.
        /// </summary>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertTo(Type type)
        {
            return typeof (DateTime) == type;
        }

        protected override object ConvertCore(string value, CultureInfo culture, Type targetType)
        {
            culture = culture ?? CultureInfo.InvariantCulture;
            return string.IsNullOrEmpty(format) ? DateTime.Parse(value, culture) : DateTime.ParseExact(value, format, culture);
        }
    }

    /// <summary>
    /// Конвертер для значений типа <see cref="TimeSpan"/>.
    /// </summary>
    public class TimeSpanConverter : BaseNullableConverter
    {
        /// <summary>
        /// Формат представления временных интервалов.
        /// </summary>
        readonly string format;

        /// <summary>
        /// Создаёт новый экземпляр конвертера для типа <see cref="TimeSpan"/> с использованием формата.
        /// </summary>
        /// <param name="format">Формат представления значений.</param>
        public TimeSpanConverter(string format = "")
        {
            this.format = format;
        }

        /// <summary>
        /// Возвращает true, если текущий конвертер может выполнять преобразования в указанный тип данных.
        /// </summary>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertTo(Type type)
        {
            return typeof(TimeSpan) == type;
        }

        protected override object ConvertCore(string value, CultureInfo culture, Type targetType)
        {
            return string.IsNullOrWhiteSpace(format) ? TimeSpan.Parse(value, culture) : TimeSpan.ParseExact(value, format, culture);
        }
    }

    /// <summary>
    /// Конвертер для преобразования значений типа <see cref="bool"/>.
    /// </summary>
    public class BooleanConverter : BaseNullableConverter
    {
        /// <summary>
        /// Возвращает true, если текущий конвертер может выполнять преобразования в указанный тип данных.
        /// </summary>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertTo(Type type)
        {
            return typeof(bool) == type;
        }

        protected override object ConvertCore(string value, CultureInfo culture, Type targetType)
        {
            return bool.Parse(value);
        }
    }

    /// <summary>
    /// Конвертер для преобразования перечислений (<see cref="Enum"/>).
    /// </summary>
    public class EnumConverter : BaseNullableConverter
    {
        /// <summary>
        /// Возвращает true, если текущий конвертер может выполнять преобразования в указанный тип данных.
        /// </summary>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertTo(Type type)
        {
            return type.IsEnum;
        }

        protected override object ConvertCore(string value, CultureInfo culture, Type targetType)
        {
            return Enum.Parse(targetType, value, true);
        }
    }

    /// <summary>
    /// Конвертер, не выполняющий никаких преобразований.
    /// </summary>
    public class TransparentConverter : IConverter
    {
        /// <summary>
        /// Экземпляр конвертера.
        /// </summary>
        public static readonly TransparentConverter Instance = new TransparentConverter();

        /// <summary>
        /// Singleton-конструктор.
        /// </summary>
        TransparentConverter()
        {
        }

        /// <summary>
        /// Возвращает true, если текущий конвертер может выполнять преобразования в указанный тип данных.
        /// </summary>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertTo(Type type)
        {
            return type == typeof (string);
        }

        public object ConvertFromString(string value, CultureInfo culture, Type targetType)
        {
            return value;
        }
    }

    /// <summary>
    /// Конвертер, выполняющий преобразование строки в коллекцию элементов.
    /// </summary>
    public class CollectionConverter : IConverter
    {
        /// <summary>
        /// Разделитель элементов коллекции по умолчанию.
        /// </summary>
        public const string DEFAULT_SEPARATOR = ",";

        /// <summary>
        /// Разделитель элементов коллекции.
        /// </summary>
        readonly string separator;

        /// <summary>
        /// Конвертер, используемый для преобразования элементов.
        /// </summary>
        readonly IConverter elementConverter;

        /// <summary>
        /// Создаёт новый экземпляр конвертера с указанием разделителя элементов и конвертером для элементов коллекции.
        /// </summary>
        /// <param name="separator">Разделитель элементов коллекции.</param>
        /// <param name="elementConverter">Конвертер для элементов коллекции.</param>
        public CollectionConverter(string separator, IConverter elementConverter)
        {
            this.separator = string.IsNullOrEmpty(separator) ? DEFAULT_SEPARATOR : separator;
            this.elementConverter = elementConverter;
        }

        /// <summary>
        /// Создаёт новый экземпляр конвертера с разделителем по умолчанию и конвертером для элементов.
        /// </summary>
        /// <param name="elementConverter">Конвертер для элементов коллекции.</param>
        public CollectionConverter(IConverter elementConverter) : this(DEFAULT_SEPARATOR, elementConverter)
        {
        }

        /// <summary>
        /// Создаёт новый экземпляр конвертера с разделителем по умолчанию.
        /// </summary>
        public CollectionConverter() : this(DEFAULT_SEPARATOR)
        {
        }

        /// <summary>
        /// Создаёт новый экземпляр конвертера
        /// </summary>
        /// <param name="separator"></param>
        public CollectionConverter(string separator) : this(separator, null)
        {
            this.separator = separator;
        }

        /// <summary>
        /// Возвращает true, если конвертер может выполнить преобразование значения в указанный тип.
        /// </summary>
        /// <remarks>Конвертер определяет, является ли переданный тип является коллекцией элементов, реализующей <see cref="IList"/> или массивом.</remarks>
        /// <param name="type">Тип, преобразование в экземпляр которого запрашивается.</param>
        public static bool CanConvertFrom(Type type)
        {
            return type.IsArray || typeof (IList).IsAssignableFrom(type);
        }

        /// <summary>
        /// Возвращает тип элемента коллекции. Если определить тип не удаётся, возвращает <see cref="string"/>.
        /// </summary>
        public static Type GetElementType(Type collectionType)
        {
            if (!CanConvertFrom(collectionType))
                throw new InvalidOperationException(string.Format("CollectionConverter is unable to convert values from string to instances of type '{0}'", collectionType.Name));
            if (collectionType.IsArray)
                return collectionType.GetElementType();
            if (collectionType.IsGenericType)
                return collectionType.GetGenericArguments()[0];
            return typeof (string);
        }

        public object ConvertFromString(string value, CultureInfo culture, Type targetType)
        {
            var items = string.IsNullOrEmpty(value) ? new string[0] : value.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
            var elementType = GetElementType(targetType);
            var convertedItems = ConvertItems(items, culture, elementType);
            return CollectItems(convertedItems, targetType);
        }

        /// <summary>
        /// Преобразует значения массива в значения элементов коллекции.
        /// </summary>
        /// <param name="source">Массив строковых значений.</param>
        /// <param name="culture">Региональные настройки, используемые при преобразовании.</param>
        /// <param name="elementType">Тип элемента результирующей коллекции.</param>
        /// <returns>Массив объектов.</returns>
        ArrayList ConvertItems(string[] source, CultureInfo culture, Type elementType)
        {
            var result = new ArrayList(source.Length);
            foreach (var s in source)
                result.Add(elementConverter == null ? s : elementConverter.ConvertFromString(s, culture, elementType));
            return result;
        }

        /// <summary>
        /// Преобразует массив элементов в результирующую коллекцию.
        /// </summary>
        /// <param name="objects">Исходный массив объектов.</param>
        /// <param name="collectionType">Тип результирующей коллекции.</param>
        static object CollectItems(ArrayList objects, Type collectionType)
        {
            if (collectionType.IsArray)
                return objects.ToArray(collectionType.GetElementType());
            var collection = (IList) Activator.CreateInstance(collectionType);
            foreach (var o in objects)
                collection.Add(o);
            return collection;
        }
    }
}
