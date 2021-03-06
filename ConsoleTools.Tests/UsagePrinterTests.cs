﻿using System;
using System.Collections;
using System.Collections.Generic;
using ConsoleTools.Binding;
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
        [TestCase(typeof(WsdlGeneratorModel))]
        public void Test(Type modelType)
        {
            var printer = new UsagePrinter(modelType, new DataProvider());
            Console.WriteLine(printer.Print());
        }
    }

    /// <summary>
    /// Модель входных аргументов для генератора сборок по wsdl.
    /// </summary>
    [NamedArgumentsPolicy("/", '=')]
    public class WsdlGeneratorModel
    {
        /// <summary>
        /// URL места, где расположен файл с WSDL сервиса.
        /// </summary>
        [Named("wsdl", IsRequired = true, Description = "URL which locates source wsdl to generate assembly.")]
        public string WsdlUrl { get; set; }

        /// <summary>
        /// Пространство имён, в котором должна быть создана сборка.
        /// </summary>
        [Named("namespace;ns", DefaultValue = "SomeAssembly.Data", Description = "Namespace containing generated classes")]
        public string RootNamespace { get; set; }

        /// <summary>
        /// Название сборки с результатом.
        /// </summary>
        [Named("assembly;asm", DefaultValue = "Data.dll", Description = "Output assembly file name (including extension).")]
        public string AssemblyName { get; set; }

        /// <summary>
        /// Версия сборки.
        /// </summary>
        [Named("version;v", IsRequired = true, Description = "Assembly version in <Major>.<Minor>.<Build>.<Revision> format.")]
        public string Version { get; set; }
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