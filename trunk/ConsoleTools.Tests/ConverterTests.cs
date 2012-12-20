using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ConsoleTools.Conversion;
using NUnit.Framework;


namespace ConsoleTools.Tests {
    [TestFixture]
    public class ConverterTests {
        #region Data

        #endregion

        #region Lifecycle

        #endregion

        #region Tests

        [Test]
        public void TestDefaultConversionToString() {
            const string testValue = "testValue";
            AssertDefaultConversion(testValue, testValue);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestDefaultConversionToInt() {
            int testValue = new Random().Next();
            AssertDefaultConversion(testValue.ToString(), testValue);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestDefaultConversionToDouble() {
            const double testValue = 1.2;
            AssertDefaultConversion(testValue.ToString(CultureInfo.InvariantCulture), testValue);
        }

        //----------------------------------------------------------------------[] 
        [Test]
        public void TestDefaultConversionToBool() {
            bool testValue = new Random().Next()%2 == 0;
            AssertDefaultConversion(testValue.ToString(), testValue);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestSeparatedValuesWithoutItemConversionToArray() {
            const string testValue = "1,2,3";
            ValueListConverterAttribute converter = new ValueListConverterAttribute();
            converter.Separator = ",";
            object result = converter.Convert(testValue, typeof (string[]));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(typeof (string[]), result);
            string[] array = (string[]) result;
            Assert.AreEqual(3, array.Length);
            Assert.AreEqual("1", array[0]);
            Assert.AreEqual("2", array[1]);
            Assert.AreEqual("3", array[2]);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestSeparatedValuesWithoutItemConversionToList() {
            const string testValue = "1,2,3";
            ValueListConverterAttribute converter = CreateCommaSeparatedListConverter();
            object result = converter.Convert(testValue, typeof (List<string>));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(typeof (List<string>), result);
            List<string> list = (List<string>) result;
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("1", list[0]);
            Assert.AreEqual("2", list[1]);
            Assert.AreEqual("3", list[2]);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestSeparatedValuesWithItemConversion() {
            const string testValue = "1,2,3";
            ValueListConverterAttribute converter = CreateCommaSeparatedListConverter(typeof (int));

            object result = converter.Convert(testValue, typeof (int[]));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(typeof (int[]), result);

            int[] array = (int[]) result;
            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestEncodingConverter() {
            Encoding e = Encoding.UTF7;
            string encodingName = e.HeaderName;

            EncodingConverterAttribute converter = new EncodingConverterAttribute();
            object result = converter.Convert(encodingName, typeof (Encoding));

            Assert.IsInstanceOfType(typeof (Encoding), result);
            Assert.AreEqual(e, result);

        }
        #endregion

        #region Routines

        private static void AssertDefaultConversion(string value, object expectedValue) {
            object result = DefaultOptionConverterAttribute.Instance.Convert(value, expectedValue.GetType());
            Assert.AreEqual(expectedValue, result);
        }

        //----------------------------------------------------------------------[]
        private static ValueListConverterAttribute CreateCommaSeparatedListConverter() {
            ValueListConverterAttribute converter = new ValueListConverterAttribute();
            converter.Separator = ",";
            return converter;
        }

        //----------------------------------------------------------------------[]
        private static ValueListConverterAttribute CreateCommaSeparatedListConverter(Type itemType) {
            ValueListConverterAttribute converter = new ValueListConverterAttribute(itemType);
            converter.Separator = ",";
            return converter;
        }

        #endregion
    }
}