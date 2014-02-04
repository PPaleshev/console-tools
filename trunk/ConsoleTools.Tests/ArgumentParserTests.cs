using NUnit.Framework;


namespace ConsoleTools.Tests {

    /// <summary>
    /// “есты дл€ <see cref="ArgumentParser"/>.
    /// </summary>
    [TestFixture]
    public class ArgumentParserTests {

        /// <summary>
        /// ѕровер€ет, что свойства с определЄнными префиксами распознаютс€ как именованные аргументы.
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
        /// ѕровер€ет, что свойства без префиксов должны распознаватьс€ как неприв€занные аргументы.
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
        /// ѕровер€ет, что разбор пустой командной строки успешно выполн€етс€. 
        /// </summary>
        [Test]
        public void ParsingNoArgumentsShouldNotFail()
        {
            var parser = new ArgumentParser();
            var args = parser.Parse(new string[0]);
            Assert.AreEqual(0, args.UnboundValues.Count);
        }

        /// <summary>
        /// ѕровер€ет, что среди аргументов содержитс€ именованный аргумент с указанным именем.
        /// </summary>
        /// <param name="args">–азобранное представление аргументов.</param>
        /// <param name="paramName">»м€ аргумента.</param>
        /// <param name="expectedValue">ќжидаемое значение аргумента.</param>
        private static void AssertNamedArgument(CmdArgs args, string paramName, string expectedValue)
        {
            string temp;
            Assert.IsTrue(args.TryGetNamedValue(paramName, out temp));
            Assert.IsTrue(args.Contains(paramName));
            Assert.AreEqual(expectedValue, temp);
        }

        /// <summary>
        /// ѕровер€ет, что указанное значение содержитс€ в списке несв€занных аргументов.
        /// </summary>
        /// <param name="args">–азобранное представление аргументов.</param>
        /// <param name="value">«начение аргумента.</param>
        private static void AssertUnboundArgument(CmdArgs args, string value)
        {
            Assert.IsTrue(args.UnboundValues.Contains(value));
        }
    }
}