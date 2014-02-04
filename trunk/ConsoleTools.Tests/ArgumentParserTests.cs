using NUnit.Framework;


namespace ConsoleTools.Tests {

    /// <summary>
    /// ����� ��� <see cref="ArgumentParser"/>.
    /// </summary>
    [TestFixture]
    public class ArgumentParserTests {

        /// <summary>
        /// ���������, ��� �������� � ������������ ���������� ������������ ��� ����������� ���������.
        /// </summary>
        [Test]
        public void PropertiesWithSpecifiedPrefixesShouldBeParsedAsNamedArguments() {
            var prefixes = new[] {"-D","--"};
            var separators = new[] {'=', ':'};
            var args = new[] {
                "-Dvalue1=something",
                "-Dvalue2:alpha",
                "--value3=17",
                "--value4",
                "/habra",
                "-babylon",
                @"c:\file.txt"
            };

            var parser = new ArgumentParser(prefixes, separators);
            parser.Parse(args);

            var result = parser.Parse(args);

            AssertNamedArgument(result, "value1", "something");
            AssertNamedArgument(result, "value2", "alpha");
            AssertNamedArgument(result, "value3", "17");
            AssertNamedArgument(result, "value4", "");
            AssertUnboundArgument(result,"/habra");
            AssertUnboundArgument(result,"-babylon");
            AssertUnboundArgument(result, @"c:\file.txt");
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� �������� ��� ��������� ������ �������������� ��� ������������� ���������.
        /// </summary>
        [Test]
        public void PropertiesWithNoPrefixesShouldBeParsedAsUnboundArguments()
        {
            var prefixes = new[] {"--"};
            var separators = new[] {'='};
            var args = new[]
                       {
                           "--value1=17",
                           "value2",
                           "value3"
                       };

            var parser = new ArgumentParser(prefixes, separators);
            var result = parser.Parse(args);

            Assert.AreEqual(2, result.UnboundValues.Count);
            AssertUnboundArgument(result, "value2");
            AssertUnboundArgument(result, "value3");
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������, ��� ������ ������ ��������� ������ ������� �����������. 
        /// </summary>
        [Test]
        public void ParsingNoArgumentsShouldNotFail()
        {
            var parser = new ArgumentParser();
            var args = parser.Parse(new string[0]);
            Assert.AreEqual(0, args.UnboundValues.Count);
        }

        /// <summary>
        /// ���������, ��� ����� ���������� ���������� ����������� �������� � ��������� ������.
        /// </summary>
        /// <param name="args">����������� ������������� ����������.</param>
        /// <param name="paramName">��� ���������.</param>
        /// <param name="expectedValue">��������� �������� ���������.</param>
        private static void AssertNamedArgument(CmdArgs args, string paramName, string expectedValue)
        {
            string temp;
            Assert.IsTrue(args.TryGetNamedValue(paramName, out temp));
            Assert.IsTrue(args.Contains(paramName));
            Assert.AreEqual(expectedValue, temp);
        }

        /// <summary>
        /// ���������, ��� ��������� �������� ���������� � ������ ����������� ����������.
        /// </summary>
        /// <param name="args">����������� ������������� ����������.</param>
        /// <param name="value">�������� ���������.</param>
        private static void AssertUnboundArgument(CmdArgs args, string value)
        {
            Assert.IsTrue(args.UnboundValues.Contains(value));
        }
    }
}