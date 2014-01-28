using System;
using System.Linq;
using ConsoleTools.Binding;
using ConsoleTools.Exceptions;
using ConsoleTools.Tests.Data;
using NUnit.Framework;


namespace ConsoleTools.Tests {
    /// <summary>
    /// ����� �������� ���������� ����������.
    /// </summary>
    [TestFixture]
    public class OptionBindingTests
    {
        /// <summary>
        /// ������ ����������.
        /// </summary>
        ArgumentParser parser;

        [TestFixtureSetUp]
        public void Setup()
        {
            parser = new ArgumentParser(new[] {"/"}, new[] {'='});
        }

        /// <summary>
        /// ��������� �������� ���������� �� �����.
        /// </summary>
        [Test]
        public void TestArgumentBindingByName()
        {
            var options = ParseAndBindTo<SampleOptions>("/stringvalue=123456");

            Assert.AreEqual("123456", options.StringValue);
            Assert.IsFalse(options.IntValue.HasValue);
            Assert.IsFalse(options.BoolValue.HasValue);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ���������� �� ���������� (alias).
        /// </summary>
        [Test]
        public void TestArgumentBindingByAlias()
        {
            var options = ParseAndBindTo<SampleOptions>("/s=123456");

            Assert.AreEqual("123456", options.StringValue);
            Assert.IsFalse(options.IntValue.HasValue);
            Assert.IsFalse(options.BoolValue.HasValue);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� "��������������" (������).
        /// </summary>
        [Test]
        public void TestSwitchBinding()
        {
            var options = ParseAndBindTo<SampleOptions>("/flagvalue");
            Assert.IsTrue(options.FlagValue.HasValue);
            Assert.IsTrue(options.FlagValue.Value);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� ����� ������ ������������� � ����������� �� �������\���������� � ������ ����������.
        /// </summary>
        [Test]
        public void TestSwitchPropertyAlwaysHasTrueOrFalseValue() {
            var options = ParseAndBindTo<SampleOptions>();

            Assert.IsTrue(options.FlagValue.HasValue);
            Assert.IsFalse(options.FlagValue.Value);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ����������� ����������.
        /// </summary>
        [Test]
        public void TestPositionalOptionBinding()
        {
            var options = ParseAndBindTo<SampleOptions>("72", "some unbound option", "extra option");

            Assert.IsTrue(options.PositionalOption1.HasValue);
            Assert.AreEqual(72, options.PositionalOption1);
            Assert.AreEqual("some unbound option", options.PositionalOption2);
            Assert.IsTrue(options.UnboundOptions.Any(s => s.Equals("extra option")));
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� ��� ������������� ��������� ����������� �������� ������������� ����������.
        /// </summary>
        [Test]
        [ExpectedException(typeof(PositionalBindingException))]
        public void TestRequiredPositionalOptionBinding()
        {
            ParseAndBindTo<RequiredPositionalOptions>();
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� ��� ������������� ����� ���������� � ����������� ��������(���� ��� �������).
        /// </summary>
        [Test]
        public void TestUnboundOptionsBinding()
        {
            var options = ParseAndBindTo<SampleOptions>("1", "posOpt2", "unbound option 1", "unbound option 2");
            Assert.IsNotNull(options.UnboundOptions, "Unbound options are null");
            Assert.AreEqual(2, options.UnboundOptions.Length);
            Assert.AreEqual("unbound option 1", options.UnboundOptions[0]);
            Assert.AreEqual("unbound option 2", options.UnboundOptions[1]);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� �������������� ����� ��� �������� �������� ����������.
        /// </summary>
        [Test]
        public void TestArgumentValueDefaultConversion()
        {
            var options = ParseAndBindTo<SampleOptions>("/intvalue=37", "/boolvalue=true", "/floatvalue=2.2", "/doublevalue=729237.188", "/timespan=00:05:00");

            Assert.IsTrue(options.IntValue.HasValue);
            Assert.AreEqual(37, options.IntValue);
            Assert.AreEqual(2.2, options.FloatValue);
            Assert.AreEqual(729237.188, options.DoubleValue);
            Assert.IsNotNull(options.TimeSpanValue);
            Assert.AreEqual(TimeSpan.FromMinutes(5), options.TimeSpanValue.Value);
            Assert.IsTrue(options.BoolValue.HasValue);
            Assert.IsTrue(options.BoolValue.Value);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� ��� ���������� ������������ ��������� ������������� ����������.
        /// </summary>
        [Test]
        [ExpectedException(typeof (MissingRequiredOptionException))]
        public void TestMissingRequiredArgumentThrowsException() {
            ParseAndBindTo<MandatoryOptions>();
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� ��� ������� ����� ������ ��������, ����������� <see cref="UnboundOptionsAttribute"/>, ������������� ����������.
        /// </summary>
        [Test]
        [ExpectedException(typeof (BindingException), "Only one property can contain unbound options")]
        public void TestOptionsObjectCanContainOnlyOnePropertyToBindUnboundArguments() {
            ParseAndBindTo<OptionsWithTwoUnboundOptionsProperties>("a", "b");
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� ��� ���������� �������� ���������, ������������ �������� �� ���������.
        /// </summary>
        [Test]
        public void TestMissingOptionalAttributeDefaultValue() {
            var opt = ParseAndBindTo<MandatoryOptions>("/required=1");
            Assert.AreEqual("bazzinga", opt.OptionalValue);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� ���������� ��������� �������� ��� �������������� ���������.
        /// </summary>
        [Test]
        public void TestDefaultListBindingWithoutConversion() {
            var opts = ParseAndBindTo<ListOptions>("/default=1,2,3,4");

            Assert.IsNotNull(opts);
            Assert.IsNotNull(opts.Default);
            Assert.AreEqual(4, opts.Default.Length);
            Assert.AreEqual("1", opts.Default[0]);
            Assert.AreEqual("2", opts.Default[1]);
            Assert.AreEqual("3", opts.Default[2]);
            Assert.AreEqual("4", opts.Default[3]);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� ���������� ������� ������.
        /// </summary>
        [Test]
        public void TestEmptyListBinding()
        {
            var opts = ParseAndBindTo<ListOptions>("/default=");
            Assert.IsNotNull(opts);
            Assert.IsNotNull(opts.Default);
            Assert.AreEqual(0, opts.Default.Length);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� ���������� ��������� ������ � ������.
        /// </summary>
        [Test]
        public void TestListBindingWithArrayConversion() {
            var opts = ParseAndBindTo<ListOptions>("/default=foo, bar, bazz");

            Assert.IsNotNull(opts);
            Assert.IsNotNull(opts.Default);
            Assert.AreEqual(3, opts.Default.Length);
            Assert.AreEqual("foo", opts.Default[0]);
            Assert.AreEqual(" bar", opts.Default[1]);
            Assert.AreEqual(" bazz", opts.Default[2]);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� ���������� ��������� ������ � generic-���� � ��������������� ��������� ���������.
        /// </summary>
        [Test]
        public void TestListBindingWithGenericListConversion()
        {
            var opts = ParseAndBindTo<ListOptions>("/list=1,2,-9,4");

            Assert.IsNotNull(opts);
            Assert.IsNotNull(opts.IntList);
            Assert.AreEqual(4, opts.IntList.Count);
            Assert.AreEqual(1, opts.IntList[0]);
            Assert.AreEqual(2, opts.IntList[1]);
            Assert.AreEqual(-9, opts.IntList[2]);
            Assert.AreEqual(4, opts.IntList[3]);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� ���������� ������������ ��������� � ������ � ��������������� ����.
        /// </summary>
        [Test]
        public void TestListPositionalBindingWithConversion() {
            var opts = ParseAndBindTo<ListOptions>("true,false,false,true");

            Assert.IsNotNull(opts);
            Assert.IsNotNull(opts.Positional);
            Assert.AreEqual(4, opts.Positional.Length);
            Assert.IsTrue(opts.Positional[0]);
            Assert.IsFalse(opts.Positional[1]);
            Assert.IsFalse(opts.Positional[2]);
            Assert.IsTrue(opts.Positional[3]);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� ���������� ��������� ��������� � ���� � ��������� ����������� ��� ���������.
        /// </summary>
        [Test]
        public void TestListBindingWithSpecifiedSeparator() {
            var options = ParseAndBindTo<ListOptions>("/separated=1_2_221_-7");   
            
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.SeparatedList);
            Assert.AreEqual(4, options.SeparatedList.Count);
            Assert.AreEqual(1,options.SeparatedList[0]);
            Assert.AreEqual(2,options.SeparatedList[1]);
            Assert.AreEqual(221,options.SeparatedList[2]);
            Assert.AreEqual(-7,options.SeparatedList[3]);
        }

        /// <summary>
        /// ��������� ������ ���������� � ����������� ��� � ������� ���������� ����.
        /// </summary>
        /// <typeparam name="TOptions">��� ������� ��� �������� ���������� ��������� ������.</typeparam>
        /// <param name="args">������ ���������� ����������.</param>
        private TOptions ParseAndBindTo<TOptions>(params string[] args) where TOptions : new() {
            var cmdargs = parser.Parse(args);
            return OptionsBinder.BindTo<TOptions>(cmdargs);
        }
    }
}