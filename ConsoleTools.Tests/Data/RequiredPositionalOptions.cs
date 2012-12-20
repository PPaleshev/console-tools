using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    public class RequiredPositionalOptions {
        #region Data

        private string _value;

        #endregion

        #region Properties

        [PositionalOption(0, true)]
        public string Value {
            get { return _value; }
            set { _value = value; }
        }

        #endregion
    }
}