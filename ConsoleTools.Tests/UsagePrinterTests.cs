using System;
using System.Collections;
using System.Collections.Generic;
using ConsoleTools.Tests.Data;
using NUnit.Framework;

namespace ConsoleTools.Tests
{
    /// <summary>
    /// Тесты печати информации о приложении.
    /// </summary>
    [TestFixture]
    public class UsagePrinterTests
    {
        [TestCase(typeof(UsageModel))]
        [TestCase(typeof(LongestWordModel))]
        public void Test(Type modelType)
        {
            var printer = new UsagePrinter(modelType, new DataProvider());
            Console.WriteLine(printer.Print());
        }

        [Test]
        public void Write()
        {
            Assert.True(typeof(IList).IsAssignableFrom(typeof(List<string>)));
        }
    }

    class DataProvider : IApplicationDataProvider
    {
        public string ApplicationExeName
        {
            get { return "UsagePrinter.exe"; }
        }

        public string Title
        {
            get { return "UsagePrinter"; }
        }

        public string Version
        {
            get { return "1.0b"; }
        }

        public string Description
        {
            get { return "Provides tools for automated usage printing of specified application."; }
        }

        public string Copyright
        {
            get { return "Copyright Nogard 2012 (C)"; }
        }
    }
}