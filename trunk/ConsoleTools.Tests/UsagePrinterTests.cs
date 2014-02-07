using System;
using System.Reflection;
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
        [Test]
        public void Test()
        {
            var printer = new UsagePrinter(typeof(SampleModel), false);
            Console.WriteLine(printer.Print());
        }
    }
}