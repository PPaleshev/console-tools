using System;
using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    /// <summary>
    /// Опции для тестирования.
    /// </summary>
    public class SampleModel
    {
        [Named("stringvalue;s")]
        public string StringValue { get; set; }

        //----------------------------------------------------------------------[]
        [Named("intvalue;i")]
        public int? IntValue { get; set; }

        //----------------------------------------------------------------------[]
        [Named("floatvalue;fl")]
        public float FloatValue { get; set; }

        //----------------------------------------------------------------------[]
        [Named("doublevalue;d")]
        public double DoubleValue { get; set; }

        //----------------------------------------------------------------------[]
        [Named("timespan;ts")]
        public TimeSpan? TimeSpanValue { get; set; }

        //----------------------------------------------------------------------[]
        [Named("boolvalue;b")]
        public bool? BoolValue { get; set; }

        //----------------------------------------------------------------------[]
        [Named("flagvalue;f", IsSwitch = true)]
        public bool? FlagValue { get; set; }

        //----------------------------------------------------------------------[]
        [Unbound("")]
        public string[] UnboundOptions { get; set; }

        //----------------------------------------------------------------------[]
        [Positional("positional1", 0)]
        public int? PositionalOption1 { get; set; }

        //----------------------------------------------------------------------[]
        [Positional("positional2", 1)]
        public string PositionalOption2 { get; set; }
    }
}