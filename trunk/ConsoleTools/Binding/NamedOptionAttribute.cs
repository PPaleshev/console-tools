using ConsoleTools.Utils;


namespace ConsoleTools.Binding {
    public class NamedOptionAttribute : OptionBindingAttribute {
        #region Data

        private readonly OptionKey _key;

        #endregion

        #region Properties

        /// <summary>
        /// Ключ аргумента
        /// </summary>
        public OptionKey Key {
            get { return _key; }
        }

        #endregion

        #region Construction

        public NamedOptionAttribute(string key, bool isRequired) : base(isRequired) {
            _key = key;
        }

        //----------------------------------------------------------------------[]
        public NamedOptionAttribute(string key) : this(key, false) {
        }

        #endregion

        #region Overrides

        public override void FillMetadata(OptionMetadata metadata) {
            metadata.Key = _key;
            metadata.ArgumentType = ArgumentType.Named;
        }

        #endregion
    }
}