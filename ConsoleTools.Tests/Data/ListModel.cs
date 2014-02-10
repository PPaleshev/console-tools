using System.Collections.Generic;
using System.ComponentModel;
using ConsoleTools.Binding;
using ConsoleTools.Conversion;

namespace ConsoleTools.Tests.Data
{
    /// <summary>
    /// Опции для проверки привязки списков.
    /// </summary>
    public class ListModel
    {
        [Named("default;d")]
        [TypeConverter(typeof(ValueListConverter))]
        public string[] Default { get; set; }

        [Named("list")]
        [TypeConverter(typeof(ValueListConverter))]
        public List<int> IntList { get; set; }

        [TypeConverter(typeof(ValueListConverter))]
        [Positional("positional list", 0)]
        public bool[] Positional { get; set; }

        [Named("separated")]
        [CollectionItemSeparator("_")]
        [TypeConverter(typeof(ValueListConverter))]
        public List<int> SeparatedList { get; set; }
    }
}