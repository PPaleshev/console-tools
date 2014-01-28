using System.Collections.Generic;
using System.ComponentModel;
using ConsoleTools.Binding;
using ConsoleTools.Conversion;

namespace ConsoleTools.Tests.Data
{
    /// <summary>
    /// Опции для проверки привязки списков.
    /// </summary>
    public class ListOptions
    {
        [NamedOption("default;d")]
        [TypeConverter(typeof(ValueListConverter))]
        public string[] Default { get; set; }

        [TypeConverter(typeof(ValueListConverter))]
        [NamedOption("list")]
        public List<int> IntList { get; set; }

        [TypeConverter(typeof(ValueListConverter))]
        [PositionalOption(0)]
        public bool[] Positional { get; set; }

        [NamedOption("separated")]
        [TypeConverter(typeof(ValueListConverter))]
        [CollectionItemSeparator("_")]
        public List<int> SeparatedList { get; set; }
    }
}