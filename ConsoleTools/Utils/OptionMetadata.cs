using System.ComponentModel;
using ConsoleTools.Binding;


namespace ConsoleTools.Utils {
    public class OptionMetadata {
        #region Data

        private OptionKey _key;
        private bool _isRequired;
        private bool _isFlag;
        private int _order;
        private readonly PropertyDescriptor _property;
        private ArgumentType _argumentType = ArgumentType.Unbound;
        private int _position;

        #endregion

        #region Properties

        public OptionKey Key {
            get { return _key; }
            set { _key = value; }
        }

        //----------------------------------------------------------------------[]
        public bool IsRequired {
            get { return _isRequired; }
            set { _isRequired = value; }
        }

        //----------------------------------------------------------------------[]
        public bool IsFlag {
            get { return _isFlag; }
            set { _isFlag = value; }
        }

        //----------------------------------------------------------------------[]
        public string Description {
            get { return _property.Description; }
        }

        //----------------------------------------------------------------------[]
        public int Order {
            get { return _order; }
            set { _order = value; }
        }

        //----------------------------------------------------------------------[]
        public ArgumentType ArgumentType {
            get { return _argumentType; }
            set { _argumentType = value; }
        }

        //----------------------------------------------------------------------[]
        public int Position {
            get { return _position; }
            set { _position = value; }
        }

        //----------------------------------------------------------------------[]
        public PropertyDescriptor PropertyDescriptor {
            get { return _property; }
        }

        #endregion

        #region Construction

        public OptionMetadata(PropertyDescriptor property) {
            _property = property;
        }

        #endregion
    }
}