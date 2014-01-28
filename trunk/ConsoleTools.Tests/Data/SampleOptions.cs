using System;
using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    /// <summary>
    /// Опции для тестирования.
    /// </summary>
    public class SampleOptions {
        [NamedOption("stringvalue;s")]
        public string StringValue { get; set; }

        //----------------------------------------------------------------------[]
        [NamedOption("intvalue;i")]
        public int? IntValue { get; set; }

        //----------------------------------------------------------------------[]
        [NamedOption("floatvalue;fl")]
        public float FloatValue { get; set; }

        //----------------------------------------------------------------------[]
        [NamedOption("doublevalue;d")]
        public double DoubleValue { get; set; }

        //----------------------------------------------------------------------[]
        [NamedOption("timespan;ts")]
        public TimeSpan? TimeSpanValue { get; set; }

        //----------------------------------------------------------------------[]
        [NamedOption("boolvalue;b")]
        public bool? BoolValue { get; set; }

        //----------------------------------------------------------------------[]
        [NamedOption("flagvalue;f"), Switch]
        public bool? FlagValue { get; set; }

        //----------------------------------------------------------------------[]
        [UnboundOptions]
        public string[] UnboundOptions { get; set; }

        //----------------------------------------------------------------------[]
        [PositionalOption(0)]
        public int? PositionalOption1 { get; set; }

        //----------------------------------------------------------------------[]
        [PositionalOption(1)]
        public string PositionalOption2 { get; set; }
    }
}