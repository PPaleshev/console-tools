using System;
using ConsoleTools.Exceptions;
using ConsoleTools.Tests.Data;
using NUnit.Framework;


namespace ConsoleTools.Tests {
    [TestFixture]
    public class OptionBindingTests {
        #region Data

        private ArgumentParser _parser;

        #endregion

        #region Lifecycle

        [TestFixtureSetUp]
        public void Setup() {
            _parser = new ArgumentParser(
                new string[] {"/"},
                new char[] {'='});
        }

        #endregion

        #region Tests

        [Test]
        public void TestArgumentBindingByName() {
            SampleOptions options = ParseTo<SampleOptions>("/stringvalue=123456");

            Assert.AreEqual("123456", options.StringValue);
            Assert.IsFalse(options.IntValue.HasValue);
            Assert.IsFalse(options.BoolValue.HasValue);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestArgumentBindingByAlias() {
            SampleOptions options = ParseTo<SampleOptions>("/s=123456");

            Assert.AreEqual("123456", options.StringValue);
            Assert.IsFalse(options.IntValue.HasValue);
            Assert.IsFalse(options.BoolValue.HasValue);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestFlagValueBinding() {
            SampleOptions options = ParseTo<SampleOptions>("/flagvalue");
            Assert.IsTrue(options.FlagValue.HasValue);
            Assert.IsTrue(options.FlagValue.Value);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestFlagPropertyAlwaysHasTrueOrFalseValue() {
            SampleOptions options = ParseTo<SampleOptions>();

            Assert.IsTrue(options.FlagValue.HasValue);
            Assert.IsFalse(options.FlagValue.Value);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestPositionalOptionBinding() {
            SampleOptions options = ParseTo<SampleOptions>("72",
                                                           "some unbound option",
                                                           "extra option");

            Assert.IsTrue(options.PositionalOption1.HasValue);
            Assert.AreEqual(72, options.PositionalOption1);
            Assert.AreEqual("some unbound option", options.PositionalOption2);
        }

        //----------------------------------------------------------------------[]
        [Test]
        [ExpectedException(typeof(PositionalBindingException))]
        public void TestRequiredPositionalOptionBinding() {
            ParseTo<RequiredPositionalOptions>();
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestUnboundOptionsBinding() {
            SampleOptions options = ParseTo<SampleOptions>("1",
                                                           "posOpt2",
                                                           "unbound option 1",
                                                           "unbound option 2");
            Assert.IsNotNull(options.UnboundOptions, "Unbound options are null");
            Assert.AreEqual(2, options.UnboundOptions.Length);
            Assert.AreEqual("unbound option 1", options.UnboundOptions[0]);
            Assert.AreEqual("unbound option 2", options.UnboundOptions[1]);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestArgumentValueDefaultConversion() {
            SampleOptions options = ParseTo<SampleOptions>("/intvalue=37", "/boolvalue=true");

            Assert.IsTrue(options.IntValue.HasValue);
            Assert.AreEqual(37, options.IntValue);
            Assert.IsTrue(options.BoolValue.HasValue);
            Assert.IsTrue(options.BoolValue.Value);
        }

        //----------------------------------------------------------------------[]
        [Test]
        [ExpectedException(typeof(MissingRequiredOptionException))]
        public void TestMissingRequiredArgumentThrowsException() {
            ParseTo<MandatoryOptions>();
        }

        //----------------------------------------------------------------------[]
        [Test]
        [ExpectedException(typeof(BindingException),"Only one property can contain unbound options")]
        public void TestOptionsObjectCanContainOnlyOnePropertyToBindUnboundArguments() {
            ParseTo<OptionsWithTwoUnboundOptionsProperties>("a", "b");
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void TestMissingOptionalAttributeDefaultValue() {
            MandatoryOptions opt = ParseTo<MandatoryOptions>("/required=1");

            Assert.AreEqual("bazzinga", opt.OptionalValue);
        }

        #endregion

        #region Routines

        private TOptions ParseTo<TOptions>(params string[] args) where TOptions : new() {
            CmdArgs cmdargs = _parser.Parse(args);
            return OptionsBinder.BindTo<TOptions>(cmdargs);
        }

        #endregion
    }
}