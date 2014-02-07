using System;
using ConsoleTools.Binding;
using NUnit.Framework;


namespace ConsoleTools.Tests {
    /// <summary>
    /// Тесты для проверки преобразования ключей свойств из строки в <see cref="PropertyKey"/>.
    /// </summary>
    [TestFixture]
    public class PropertyKeyTests {

        /// <summary>
        /// Проверяет, что преобразование из пустой строки невозможно.
        /// </summary>
        [TestCase("")]
        [TestCase(null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestImplicitConversionFromString(string value)
        {
            PropertyKey key = value;
        }

        /// <summary>
        /// Проверяет, что создание ключа с пустым названием 
        /// </summary>
        [TestCase("")]
        [TestCase(null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestExplicitConstruction(string value)
        {
            var key = new PropertyKey("");
        }

        //----------------------------------------------------------------------[]
        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestNonEmptyPropertyName()
        {
            PropertyKey key = ";";
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestNameAndAlias()
        {
            PropertyKey key = "name;alias";
            Assert.AreEqual("name", key.Name);
            Assert.IsTrue(key.HasAlias);
            Assert.AreEqual("alias", key.Alias);
        }
    }
}