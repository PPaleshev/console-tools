using ConsoleTools.Binding;

namespace ConsoleTools.Exceptions {
    public class MissingRequiredOptionException : BindingException {
        #region Data

        private readonly OptionMetadata _metadata;

        #endregion

        #region Properties

        public OptionMetadata Metadata {
            get { return _metadata; }
        }

        #endregion

        #region Construction

        public MissingRequiredOptionException(OptionMetadata metadata)
            : base("Missing required option: " + metadata.Key) {
            _metadata = metadata;
        }

        #endregion
    }
}