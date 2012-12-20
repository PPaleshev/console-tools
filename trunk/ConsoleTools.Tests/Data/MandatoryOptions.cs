using System.ComponentModel;
using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    public class MandatoryOptions {
        #region Data

        private string _requiredValue,
                       _optionalValue;
        #endregion

        #region Properties

        [NamedOption("required;r", true)]
        public string RequiredValue {
            get { return _requiredValue; }
            set { _requiredValue = value; }
        }

        //----------------------------------------------------------------------[]
        [NamedOption("optional;o")]
        [DefaultValue("bazzinga")]
        public string OptionalValue {
            get { return _optionalValue; }
            set { _optionalValue = value; }
        }

        #endregion
    }
}