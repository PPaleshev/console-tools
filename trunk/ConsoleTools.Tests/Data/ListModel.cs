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
        [TypeConverter(typeof(CollectionArgumentConverter))]
        public string[] Default { get; set; }

        [Named("list")]
        [TypeConverter(typeof(CollectionArgumentConverter))]
        public List<int> IntList { get; set; }

        [TypeConverter(typeof(CollectionArgumentConverter))]
        [Positional("positional list", 0)]
        public bool[] Positional { get; set; }

        [Named("separated")]
        [CollectionItemSeparator("_")]
        [TypeConverter(typeof(CollectionArgumentConverter))]
        public List<int> SeparatedList { get; set; }
    }
}