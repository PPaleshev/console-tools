using System.ComponentModel;
using ConsoleTools.Binding;
using ConsoleTools.Conversion;

namespace ConsoleTools.Tests.Data
{
    [NamedArgumentsPolicy("-", '=')]
    public class UsageModel
    {
        [Positional("to", 1, Required = true)]
        public string To { get; set; }

        [Positional("from", 0, Required = true)]
        public string From { get; set; }

        [Positional("newname", 3)]
        [TypeConverter(typeof(CollectionArgumentConverter))]
        [CollectionItemSeparator(";")]
        public string[] NewNames { get; set; }

        [Positional("masksToCopy", 2, Required = true)]
        [TypeConverter(typeof(CollectionArgumentConverter))]
        public string[] MasksToCopy { get; set; }

        [Named("force;f", Switch = true)]
        public bool Force { get; set; }

        [Named("target;t", Required = true)]
        public int Target { get; set; }
    }
}