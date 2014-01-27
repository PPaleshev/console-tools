using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    public class SampleOptions {
        #region Data

        private string _stringValue;
        private int? _intValue;
        private bool? _boolValue;
        private bool? _flagValue;
        private string[] _unboundOptions;
        private int? _positionalOption1;
        private string _positionalOption2;

        #endregion

        #region Properties

        [NamedOption("stringvalue;s")]
        public string StringValue {
            get { return _stringValue; }
            set { _stringValue = value; }
        }

        //----------------------------------------------------------------------[]
        [NamedOption("intvalue;i")]
        public int? IntValue {
            get { return _intValue; }
            set { _intValue = value; }
        }

        //----------------------------------------------------------------------[]
        [NamedOption("boolvalue;b")]
        public bool? BoolValue {
            get { return _boolValue; }
            set { _boolValue = value; }
        }

        //----------------------------------------------------------------------[]
        [NamedOption("flagvalue;f"), Switch]
        public bool? FlagValue {
            get { return _flagValue; }
            set { _flagValue = value; }
        }

        //----------------------------------------------------------------------[]
        [UnboundOptions]
        public string[] UnboundOptions {
            get { return _unboundOptions; }
            set { _unboundOptions = value; }
        }

        //----------------------------------------------------------------------[]
        [PositionalOption(0)]
        public int? PositionalOption1 {
            get { return _positionalOption1; }
            set { _positionalOption1 = value; }
        }

        //----------------------------------------------------------------------[]
        [PositionalOption(1)]
        public string PositionalOption2 {
            get { return _positionalOption2; }
            set { _positionalOption2 = value; }
        }

        #endregion
    }
}