using System;
using System.Collections.Generic;
using ConsoleTools.Binding;
using ConsoleTools.Conversion;

namespace ConsoleTools.Tests.Data
{
    /// <summary>
    /// Опции для проверки привязки списков.
    /// </summary>
    public class ListsModel
    {
        [Named("default;d")]
        public string[] Default { get; set; }

        [Named("list")]
        public List<int> IntList { get; set; }

        [Positional("positional list", 0)]
        public bool[] Positional { get; set; }

        [Named("separated")]
        [CollectionConversion("_")]
        public List<int> SeparatedList { get; set; }

        [Named("times")]
        [CollectionConversion(" ", ElementFormatString = @"h\-m\-s")]
        public List<TimeSpan> Timespans { get; set; }
    }
}