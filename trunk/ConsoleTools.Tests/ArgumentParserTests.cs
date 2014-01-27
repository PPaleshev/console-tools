using NUnit.Framework;


namespace ConsoleTools.Tests {

    /// <summary>
    /// Тесты для <see cref="ArgumentParser"/>.
    /// </summary>
    [TestFixture]
    public class ArgumentParserTests {
        [Test]
        public void PropertiesWithSpecifiedPrefixesShouldBeParsedAsNamedArguments() {
            var prefixes = new[] {"-D","--"};
            var separators = new[] {'=', ':'};
            var args = new[] {
                "-Dvalue1=something",
                "-Dvalue2:alpha",
                "--value3=17",
                "--value4"
            };

            var parser = new ArgumentParser(prefixes, separators);
            parser.Parse(args);

            var result = parser.Parse(args);

            AssertNamedArgument(result, "value1", "something");
            AssertNamedArgument(result, "value2", "alpha");
            AssertNamedArgument(result, "value3", "17");
            AssertNamedArgument(result, "value4", "");
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void PropertiesWithNoPrefixesShouldBeParsedAsDefaultArguments()
        {
            var prefixes = new[] {"--"};
            var separators = new[] {'='};
            var args = new[]
                           {
                               "--value1=17",
                               "value2",
                               "value3"
                           };

            var parser = new ArgumentParser(prefixes,separators);
            var result = parser.Parse(args);

            Assert.AreEqual(2, result.Args.Count);
            Assert.Contains("value2", result.Args);
            Assert.Contains("value3", result.Args);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void ParsingNoArgumentsShouldNotFail()
        {
            var parser = new ArgumentParser();
            parser.Parse(new string[0]);
        }

        private static void AssertNamedArgument(CmdArgs args, string paramName, string value)
        {
            string temp;
            Assert.IsTrue(args.TryGetNamedValue(paramName, out temp));
            Assert.IsTrue(args.Contains(paramName));
            Assert.AreEqual(value, temp);
        }
    }
}