using NUnit.Framework;


namespace ConsoleTools.Tests {
    [TestFixture]
    public class ArgumentParserTests {
        #region Methods

        [Test]
        public void PropertiesWithSpecifiedPrefixesShouldBeParsedAsNamedArguments() {
            string[] prefixes = new string[] {"-D","--"};
            char[] separators = new char[] {'=', ':'};
            string[] args = new string[] {
                "-Dvalue1=something",
                "-Dvalue2:alpha",
                "--value3=17",
                "--value4"
            };


            ArgumentParser parser = new ArgumentParser();
            parser.ArgumentPrefixes = prefixes;
            parser.Separators = separators;
            parser.Parse(args);

            CmdArgs result = parser.Parse(args);

            AssertNamedArgument(result, "value1", "something");
            AssertNamedArgument(result, "value2", "alpha");
            AssertNamedArgument(result, "value3", "17");
            AssertNamedArgument(result, "value4", "");
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void PropertiesWithNoPrefixesShouldBeParsedAsDefaultArguments() {
            string[] prefixes = new string[] {"--" };
            char[] separators = new char[] {'='};
            string[] args = new string[] {
                "--value1=17",
                "value2",
                "value3"
            };

            ArgumentParser parser = new ArgumentParser();
            parser.ArgumentPrefixes = prefixes;
            parser.Separators = separators;
            CmdArgs result = parser.Parse(args);

            Assert.AreEqual(2, result.Args.Count);
            Assert.Contains("value2", result.Args);
            Assert.Contains("value3", result.Args);
        }

        //----------------------------------------------------------------------[]
        [Test]
        public void ParsingNoArgumentsShouldNotFail() {
            ArgumentParser parser = new ArgumentParser();
            parser.Parse(new string[0]);
        }

        #endregion

        #region Routines

        private static void AssertNamedArgument(CmdArgs args, string paramName, string value) {
            Assert.IsTrue(args.Contains(paramName));
            Assert.IsTrue(args[paramName] == value);
        }

        #endregion
    }
}