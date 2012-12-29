using System;
using ConsoleTools.Binding;
using NUnit.Framework;


namespace ConsoleTools.Tests {
    [TestFixture]
    public class OptionKeyTests {
        #region Methods

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void OptionKeyCannotBeEmpty() {
            OptionKey key = "";
        }

        //----------------------------------------------------------------------[]
        [Test]
        [ExpectedException(typeof (FormatException))]
        public void OptionKeyShouldContainNonEmptyName() {
            OptionKey key = ";";
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void OptionKeyCanContainNameAndAlias() {
            OptionKey key = "name;alias";

            Assert.AreEqual("name", key.Name);
            Assert.IsTrue(key.HasAlias);
            Assert.AreEqual("alias", key.Alias);
        }

        #endregion
    }
}