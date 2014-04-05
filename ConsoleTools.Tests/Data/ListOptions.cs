using System.Collections.Generic;
using System.ComponentModel;
using ConsoleTools.Binding;
using ConsoleTools.Conversion;


namespace ConsoleTools.Tests.Data {
    public class ListOptions {
        #region Data

        private object[] _default;
        private string[] _stringArray;
        private List<int> _intList;
        private bool[] _positional;
        private int[] _separatedList;

        #endregion

        #region Properties

        [NamedOption("default")]
        [TypeConverter(typeof (ValueListConverter))]
        public object[] Default {
            get { return _default; }
            set { _default = value; }
        }

        //----------------------------------------------------------------------[]
        [NamedOption("stringarray")]
        [TypeConverter(typeof (ValueListConverter))]
        public string[] StringArray {
            get { return _stringArray; }
            set { _stringArray = value; }
        }

        //----------------------------------------------------------------------[]
        [NamedOption("list")]
        [TypeConverter(typeof (ValueListConverter))]
        public List<int> IntList {
            get { return _intList; }
            set { _intList = value; }
        }

        //----------------------------------------------------------------------[]
        [PositionalOption(0)]
        [TypeConverter(typeof(ValueListConverter))]
        public bool[] Positional {
            get { return _positional; }
            set { _positional = value; }
        }

        //----------------------------------------------------------------------[]
        [TypeConverter(typeof(ValueListConverter))]
        [ListItemSeparator("_")]
        [NamedOption("separated")]
        public int[] SeparatedList {
            get { return _separatedList; }
            set { _separatedList = value; }
        }
        #endregion
    }
}