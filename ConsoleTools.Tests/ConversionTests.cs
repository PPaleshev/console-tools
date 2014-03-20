using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ConsoleTools.Conversion;
using ConsoleTools.Tests.Data;
using NUnit.Framework;
using CollectionConverter = ConsoleTools.Conversion.CollectionConverter;
using DateTimeConverter = ConsoleTools.Conversion.DateTimeConverter;
using DecimalConverter = ConsoleTools.Conversion.DecimalConverter;
using EnumConverter = ConsoleTools.Conversion.EnumConverter;
using TimeSpanConverter = ConsoleTools.Conversion.TimeSpanConverter;


namespace ConsoleTools.Tests {
    /// <summary>
    /// Тесты для встроенных конвертеров.
    /// </summary>
    [TestFixture]
    public class ConversionTests
    {
        /// <summary>
        /// Региональные настройки, используемые в тестах.
        /// </summary>
        static readonly CultureInfo CULTURE = CultureInfo.InvariantCulture;

        /// <summary>
        /// Проверяет успешные сценарии преобразования целых чисел.
        /// </summary>
        [Test]
        public void TestSuccessIntegralConversion()
        {
            var culture = CultureInfo.InvariantCulture;
            var converter = new NumberConverter();

            Assert.AreEqual(byte.MinValue, converter.ConvertFromString(byte.MinValue.ToString(culture), culture, typeof (byte)));
            Assert.AreEqual(byte.MaxValue, converter.ConvertFromString(byte.MaxValue.ToString(culture), culture, typeof (byte)));
            Assert.AreEqual(byte.MinValue, converter.ConvertFromString(byte.MinValue.ToString(culture), culture, typeof (byte?)));
            Assert.AreEqual(byte.MaxValue, converter.ConvertFromString(byte.MaxValue.ToString(culture), culture, typeof (byte?)));
            Assert.AreEqual((byte) 0, converter.ConvertFromString("0", culture, typeof (byte)));
            Assert.AreEqual((byte) 0, converter.ConvertFromString("0", culture, typeof (byte?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof (byte?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof (byte)));

            Assert.AreEqual(sbyte.MinValue, converter.ConvertFromString(sbyte.MinValue.ToString(culture), culture, typeof(sbyte)));
            Assert.AreEqual(sbyte.MaxValue, converter.ConvertFromString(sbyte.MaxValue.ToString(culture), culture, typeof(sbyte)));
            Assert.AreEqual(sbyte.MinValue, converter.ConvertFromString(sbyte.MinValue.ToString(culture), culture, typeof(sbyte?)));
            Assert.AreEqual(sbyte.MaxValue, converter.ConvertFromString(sbyte.MaxValue.ToString(culture), culture, typeof(sbyte?)));
            Assert.AreEqual((sbyte)0, converter.ConvertFromString("0", culture, typeof(sbyte)));
            Assert.AreEqual((sbyte)0, converter.ConvertFromString("0", culture, typeof(sbyte?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(sbyte?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(sbyte)));

            Assert.AreEqual(short.MinValue, converter.ConvertFromString(short.MinValue.ToString(culture), culture, typeof(short)));
            Assert.AreEqual(short.MaxValue, converter.ConvertFromString(short.MaxValue.ToString(culture), culture, typeof(short)));
            Assert.AreEqual(short.MinValue, converter.ConvertFromString(short.MinValue.ToString(culture), culture, typeof(short?)));
            Assert.AreEqual(short.MaxValue, converter.ConvertFromString(short.MaxValue.ToString(culture), culture, typeof(short?)));
            Assert.AreEqual((short)0, converter.ConvertFromString("0", culture, typeof(short)));
            Assert.AreEqual((short)0, converter.ConvertFromString("0", culture, typeof(short?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(short?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(short)));

            Assert.AreEqual(ushort.MinValue, converter.ConvertFromString(ushort.MinValue.ToString(culture), culture, typeof(ushort)));
            Assert.AreEqual(ushort.MaxValue, converter.ConvertFromString(ushort.MaxValue.ToString(culture), culture, typeof(ushort)));
            Assert.AreEqual(ushort.MinValue, converter.ConvertFromString(ushort.MinValue.ToString(culture), culture, typeof(ushort?)));
            Assert.AreEqual(ushort.MaxValue, converter.ConvertFromString(ushort.MaxValue.ToString(culture), culture, typeof(ushort?)));
            Assert.AreEqual((ushort)0, converter.ConvertFromString("0", culture, typeof(ushort)));
            Assert.AreEqual((ushort)0, converter.ConvertFromString("0", culture, typeof(ushort?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(ushort?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(ushort)));

            Assert.AreEqual(int.MinValue, converter.ConvertFromString(int.MinValue.ToString(culture), culture, typeof(int)));
            Assert.AreEqual(int.MaxValue, converter.ConvertFromString(int.MaxValue.ToString(culture), culture, typeof(int)));
            Assert.AreEqual(int.MinValue, converter.ConvertFromString(int.MinValue.ToString(culture), culture, typeof(int?)));
            Assert.AreEqual(int.MaxValue, converter.ConvertFromString(int.MaxValue.ToString(culture), culture, typeof(int?)));
            Assert.AreEqual((int)0, converter.ConvertFromString("0", culture, typeof(int)));
            Assert.AreEqual((int)0, converter.ConvertFromString("0", culture, typeof(int?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(int?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(int)));

            Assert.AreEqual(uint.MinValue, converter.ConvertFromString(uint.MinValue.ToString(culture), culture, typeof(uint)));
            Assert.AreEqual(uint.MaxValue, converter.ConvertFromString(uint.MaxValue.ToString(culture), culture, typeof(uint)));
            Assert.AreEqual(uint.MinValue, converter.ConvertFromString(uint.MinValue.ToString(culture), culture, typeof(uint?)));
            Assert.AreEqual(uint.MaxValue, converter.ConvertFromString(uint.MaxValue.ToString(culture), culture, typeof(uint?)));
            Assert.AreEqual((uint)0, converter.ConvertFromString("0", culture, typeof(uint)));
            Assert.AreEqual((uint)0, converter.ConvertFromString("0", culture, typeof(uint?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(uint?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(uint)));

            Assert.AreEqual(long.MinValue, converter.ConvertFromString(long.MinValue.ToString(culture), culture, typeof(long)));
            Assert.AreEqual(long.MaxValue, converter.ConvertFromString(long.MaxValue.ToString(culture), culture, typeof(long)));
            Assert.AreEqual(long.MinValue, converter.ConvertFromString(long.MinValue.ToString(culture), culture, typeof(long?)));
            Assert.AreEqual(long.MaxValue, converter.ConvertFromString(long.MaxValue.ToString(culture), culture, typeof(long?)));
            Assert.AreEqual((long)0, converter.ConvertFromString("0", culture, typeof(long)));
            Assert.AreEqual((long)0, converter.ConvertFromString("0", culture, typeof(long?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(long?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(long)));

            Assert.AreEqual(ulong.MinValue, converter.ConvertFromString(ulong.MinValue.ToString(culture), culture, typeof(ulong)));
            Assert.AreEqual(ulong.MaxValue, converter.ConvertFromString(ulong.MaxValue.ToString(culture), culture, typeof(ulong)));
            Assert.AreEqual(ulong.MinValue, converter.ConvertFromString(ulong.MinValue.ToString(culture), culture, typeof(ulong?)));
            Assert.AreEqual(ulong.MaxValue, converter.ConvertFromString(ulong.MaxValue.ToString(culture), culture, typeof(ulong?)));
            Assert.AreEqual((ulong)0, converter.ConvertFromString("0", culture, typeof(ulong)));
            Assert.AreEqual((ulong)0, converter.ConvertFromString("0", culture, typeof(ulong?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(ulong?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(ulong)));

            Assert.True(Math.Abs((float)converter.ConvertFromString("123.33", culture, typeof(float)) - ((float)123.33)) < (float)0.001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("0.0000002347", culture, typeof(float)) - ((float)0.0000002347)) < (float)0.00000000001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("-0.0000002347", culture, typeof(float)) - ((float)-0.0000002347)) < (float)0.00000000001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("-987347.2304757", culture, typeof(float)) - ((float)-987347.2304757)) < (float)0.00000001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("123.33", culture, typeof(float?)) - ((float)123.33)) < (float)0.001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("0.0000002347", culture, typeof(float?)) - ((float)0.0000002347)) < (float)0.00000000001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("-0.0000002347", culture, typeof(float?)) - ((float)-0.0000002347)) < (float)0.00000000001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("-987347.2304757", culture, typeof(float?)) - ((float)-987347.2304757)) < (float)0.00000001);
            Assert.True(Math.Abs((float)converter.ConvertFromString("1.227e-4", culture, typeof(float)) - ((float)1.227e-4)) < (float)0.00000001);
            Assert.True(Math.Abs((float) converter.ConvertFromString("50", culture, typeof (float)) - ((float) 50)) < (float) 0.1);
            Assert.True(Math.Abs((float)converter.ConvertFromString("-7180", culture, typeof(float)) - ((float)-7180)) < (float)0.1);
            Assert.AreEqual((float)0, converter.ConvertFromString("0", culture, typeof(float)));
            Assert.AreEqual((float)0, converter.ConvertFromString("0", culture, typeof(float?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(float?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(float)));

            Assert.True(Math.Abs((double)converter.ConvertFromString("123.33", culture, typeof(double)) - ((double)123.33)) < (double)0.001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("0.0000002347", culture, typeof(double)) - ((double)0.0000002347)) < (double)0.00000000001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("-0.0000002347", culture, typeof(double)) - ((double)-0.0000002347)) < (double)0.00000000001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("-987347.2304757", culture, typeof(double)) - ((double)-987347.2304757)) < (double)0.00000001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("123.33", culture, typeof(double?)) - ((double)123.33)) < (double)0.001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("0.0000002347", culture, typeof(double?)) - ((double)0.0000002347)) < (double)0.00000000001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("-0.0000002347", culture, typeof(double?)) - ((double)-0.0000002347)) < (double)0.00000000001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("-987347.2304757", culture, typeof(double?)) - ((double)-987347.2304757)) < (double)0.00000001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("1.227e-4", culture, typeof(double)) - ((double)1.227e-4)) < (double)0.00000001);
            Assert.True(Math.Abs((double)converter.ConvertFromString("50", culture, typeof(double)) - ((double)50)) < (double)0.1);
            Assert.True(Math.Abs((double)converter.ConvertFromString("-7180", culture, typeof(double)) - ((double)-7180)) < (double)0.1);
            Assert.AreEqual((double)0, converter.ConvertFromString("0", culture, typeof(double)));
            Assert.AreEqual((double)0, converter.ConvertFromString("0", culture, typeof(double?)));
            Assert.Null(converter.ConvertFromString("", culture, typeof(double?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", culture, typeof(double)));
        }

        /// <summary>
        /// Проверяет преобразование значений в тип <see cref="DateTime"/> без использования предустановленного формата.
        /// </summary>
        [Test]
        public void TestGeneralDateTimeConversion()
        {
            var converter = DateTimeConverter.Instance;

            Assert.Throws<FormatException>(() => converter.ConvertFromString("", CULTURE, typeof (DateTime)));
            Assert.Null(converter.ConvertFromString("", CULTURE, typeof (DateTime?)));
            Assert.AreEqual(new DateTime(2007, 11, 5), converter.ConvertFromString("11/5/2007", CULTURE, typeof (DateTime)));
            Assert.AreEqual(new DateTime(2007, 11, 5), converter.ConvertFromString("11.5.2007", CULTURE, typeof (DateTime)));
            Assert.AreEqual(new DateTime(2007, 11, 5, 22, 11, 14), converter.ConvertFromString("11.5.2007 22:11:14", CULTURE, typeof (DateTime)));
        }

        /// <summary>
        /// Проверяет преобразование значений <see cref="DateTime"/> с использованием предустановленного формата.
        /// </summary>
        [Test]
        public void TestDateTimeConversionWithFormat()
        {
            var converter = new DateTimeConverter("dd.MM.yyyy");
            
            Assert.AreEqual(new DateTime(2007, 11, 5), converter.ConvertFromString("05.11.2007", CULTURE, typeof (DateTime)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("05/11/2007", CULTURE, typeof (DateTime)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("05.11.2007 12:01:02", CULTURE, typeof (DateTime?)));
        }

        /// <summary>
        /// Проверяет преобразование значений <see cref="TimeSpan"/> без использования предустановленного формата.
        /// </summary>
        [Test]
        public void TestGeneralTimeSpanConversion()
        {
            var converter = new TimeSpanConverter();

            Assert.Null(converter.ConvertFromString("", CULTURE, typeof (TimeSpan?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", CULTURE, typeof (TimeSpan)));
            Assert.AreEqual(new TimeSpan(1, 2, 3, 4), converter.ConvertFromString("01:02:03:04", CULTURE, typeof (TimeSpan)));
            Assert.AreEqual(new TimeSpan(2, 3, 4), converter.ConvertFromString("02:03:04", CULTURE, typeof (TimeSpan)));
        }

        /// <summary>
        /// Проверяет преобразование значений типа <see cref="TimeSpan"/> с использованием предустановленного формата.
        /// </summary>
        [Test]
        public void TestTimespanConversionWithFormat()
        {
            var converter = new TimeSpanConverter(@"hh\-mm\-ss");
            Assert.AreEqual(new TimeSpan(3, 15, 22), converter.ConvertFromString("03-15-22", CULTURE, typeof (TimeSpan)));

            converter = new TimeSpanConverter(@"hh'h 'mm'm 'ss's'");
            Assert.AreEqual(new TimeSpan(2, 12, 37), converter.ConvertFromString("02h 12m 37s", CULTURE, typeof (TimeSpan)));

            converter = new TimeSpanConverter(@"h'ч 'm'мин 's'сек'");
            Assert.AreEqual(new TimeSpan(2, 12, 8), converter.ConvertFromString("2ч 12мин 8сек", CULTURE, typeof (TimeSpan)));
        }

        /// <summary>
        /// Проверяет преобразование значений с плавающей точкой.
        /// </summary>
        [Test]
        public void TestDecimalConversion()
        {
            var converter = new DecimalConverter();

            Assert.True(Math.Abs((decimal)converter.ConvertFromString("123.33", CULTURE, typeof(decimal)) - ((decimal)123.33)) < (decimal)0.001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("0.0000002347", CULTURE, typeof(decimal)) - ((decimal)0.0000002347)) < (decimal)0.00000000001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("-0.0000002347", CULTURE, typeof(decimal)) - ((decimal)-0.0000002347)) < (decimal)0.00000000001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("-987347.2304757", CULTURE, typeof(decimal)) - ((decimal)-987347.2304757)) < (decimal)0.00000001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("123.33", CULTURE, typeof(decimal?)) - ((decimal)123.33)) < (decimal)0.001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("0.0000002347", CULTURE, typeof(decimal?)) - ((decimal)0.0000002347)) < (decimal)0.00000000001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("-0.0000002347", CULTURE, typeof(decimal?)) - ((decimal)-0.0000002347)) < (decimal)0.00000000001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("-987347.2304757", CULTURE, typeof(decimal?)) - ((decimal)-987347.2304757)) < (decimal)0.00000001);
            Assert.True(Math.Abs((decimal)converter.ConvertFromString("1.227e-4", CULTURE, typeof(decimal)) - ((decimal)1.227e-4)) < (decimal)0.00000001);
            Assert.AreEqual((decimal)0, converter.ConvertFromString("0", CULTURE, typeof(decimal)));
            Assert.AreEqual((decimal)0, converter.ConvertFromString("0", CULTURE, typeof(decimal?)));
            Assert.Null(converter.ConvertFromString("", CULTURE, typeof(decimal?)));
            Assert.Throws<FormatException>(() => converter.ConvertFromString("", CULTURE, typeof(decimal)));
        }

        /// <summary>
        /// Проверяет правильность определения типа элемента коллекции.
        /// </summary>
        [Test]
        public void TestCollectionConverterElementType()
        {
            Assert.AreEqual(typeof (int), CollectionConverter.GetElementType(typeof (int[])));
            Assert.AreEqual(typeof (int), CollectionConverter.GetElementType(typeof (List<int>)));
            Assert.AreEqual(typeof (int?), CollectionConverter.GetElementType(typeof (List<int?>)));
            Assert.AreEqual(typeof (string), CollectionConverter.GetElementType(typeof (ArrayList)));
        }

        /// <summary>
        /// Проверяет правильность обработки пустых коллекций.
        /// </summary>
        /// <param name="expectedType"></param>
        [TestCase(typeof (TimeSpan[]))]
        [TestCase(typeof (List<TimeSpan>))]
        [TestCase(typeof (Collection<TimeSpan>))]
        public void TestEmptyCollection(Type expectedType)
        {
            var converter = new CollectionConverter();
            var result = converter.ConvertFromString("", CULTURE, expectedType);
            Assert.AreEqual(expectedType, result.GetType());
            Assert.IsNotNull(result);
            Assert.AreEqual(0, ((IList) result).Count);
        }

        /// <summary>
        /// Проверяет преобразование из строки в коллекции без преобразования элементов.
        /// </summary>
        [TestCaseSource("CreateCollectionConversionTestCases")]
        public void TestCollectionConversion(Type expectedType, string separator)
        {
            var converter = new CollectionConverter(separator);
            var result = converter.ConvertFromString(string.Join(separator, new[] {"1", "2", "3", "4", "5"}), CULTURE, expectedType);
            Assert.AreEqual(expectedType, result.GetType());
            var arr = (IList) result;
            Assert.AreEqual(5, arr.Count);
            Assert.AreEqual("1", arr[0]);
            Assert.AreEqual("3", arr[2]);
            Assert.AreEqual("5", arr[4]);
        }

        /// <summary>
        /// Создаёт перечисление сценариев для теста <see cref="TestCollectionConversion"/>.
        /// </summary>
        static IEnumerable<TestCaseData> CreateCollectionConversionTestCases()
        {
            foreach (var type in new[] {typeof (string[]), typeof (List<string>), typeof (ArrayList)})
            foreach (var s in new[] {"-", "--", "_", " "})
                yield return new TestCaseData(type, s);
        }

        /// <summary>
        /// Проверяет правильность преобразования в коллекцию с использованием конвертера для элементов.
        /// </summary>
        [TestCase(typeof(int[]))]
        [TestCase(typeof(List<int>))]
        [TestCase(typeof(Collection<int>))]
        public void TestCollectionWithElementConversion(Type expectedType)
        {
            var converter = new CollectionConverter(" ", new NumberConverter());
            var result = converter.ConvertFromString("1 2 3 4 5", CULTURE, expectedType);
            Assert.AreEqual(expectedType, result.GetType());
            var arr = (IList)result;
            Assert.AreEqual(5, arr.Count);
            Assert.AreEqual(1, arr[0]);
            Assert.AreEqual(3, arr[2]);
            Assert.AreEqual(5, arr[4]);
        }

        /// <summary>
        /// Проверяет преобразование пустых значений с использованием конвертера перечислений.
        /// </summary>
        [Test]
        public void TestEmptyEnumConversion()
        {
            var converter = new EnumConverter();
            Assert.Throws<ArgumentException>(() => converter.ConvertFromString("", CULTURE, typeof (FlagEnum)));
            Assert.Null(converter.ConvertFromString("", CULTURE, typeof (FlagEnum?)));

            Assert.Throws<ArgumentException>(() => converter.ConvertFromString("", CULTURE, typeof (SimpleEnum)));
            Assert.Null(converter.ConvertFromString("", CULTURE, typeof (SimpleEnum?)));
        }

        /// <summary>
        /// Проверяет преобразование перечислений, являющихся флагами.
        /// </summary>
        [Test]
        public void TestEnumFlagsConversion()
        {
            var converter = new EnumConverter();
            var result = converter.ConvertFromString("Item3, Item1", CULTURE, typeof (FlagEnum));
            Assert.IsInstanceOf(typeof (FlagEnum), result);
            var flags = (FlagEnum) result;
            Assert.True(flags.HasFlag(FlagEnum.Item1));
            Assert.True(flags.HasFlag(FlagEnum.Item3));
            Assert.False(flags.HasFlag(FlagEnum.Item2));
        }

        /// <summary>
        /// Проверяет преобразование перечислений, не являющихся флагами.
        /// </summary>
        [Test]
        public void TestEnumConversion()
        {
            var converter = new EnumConverter();
            var result = converter.ConvertFromString("Item2", CULTURE, typeof (SimpleEnum));
            Assert.AreEqual(SimpleEnum.Item2, result);
            result = converter.ConvertFromString("0", CULTURE, typeof (SimpleEnum));
            Assert.AreEqual(SimpleEnum.None, result);
        }

        [Test]
        [Explicit]
        public void Temp()
        {
        }

        [TestCase("float")]
        [TestCase("double")]
        [TestCase("decimal")]
        [Explicit]
        public void GenerateTestCase(string type)
        {
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"123.33\", culture, typeof({0})) - (({0})123.33)) < ({0})0.001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"0.0000002347\", culture, typeof({0})) - (({0})0.0000002347)) < ({0})0.00000000001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"-0.0000002347\", culture, typeof({0})) - (({0})-0.0000002347)) < ({0})0.00000000001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"-987347.2304757\", culture, typeof({0})) - (({0})-987347.2304757)) < ({0})0.00000001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"123.33\", culture, typeof({0}?)) - (({0})123.33)) < ({0})0.001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"0.0000002347\", culture, typeof({0}?)) - (({0})0.0000002347)) < ({0})0.00000000001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"-0.0000002347\", culture, typeof({0}?)) - (({0})-0.0000002347)) < ({0})0.00000000001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"-987347.2304757\", culture, typeof({0}?)) - (({0})-987347.2304757)) < ({0})0.00000001);", type);
            Console.WriteLine("Assert.True(Math.Abs(({0})converter.ConvertFromString(\"1.227e-4\", culture, typeof({0})) - (({0})1.227e-4)) < ({0})0.00000001);", type);
            Console.WriteLine("Assert.AreEqual(({0})0, converter.ConvertFromString(\"0\", culture, typeof({0})));", type);
            Console.WriteLine("Assert.AreEqual(({0})0, converter.ConvertFromString(\"0\", culture, typeof({0}?)));", type);
            Console.WriteLine("Assert.Null(converter.ConvertFromString(\"\", culture, typeof({0}?)));", type);
            Console.WriteLine("Assert.Throws<FormatException>(() => converter.ConvertFromString(\"\", culture, typeof({0})));", type);
            Console.WriteLine();
        }
    }
}