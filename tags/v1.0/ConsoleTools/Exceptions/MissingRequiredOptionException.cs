using ConsoleTools.Binding;

namespace ConsoleTools.Exceptions {
    public class MissingRequiredOptionException : BindingException {
        #region Data

        private readonly PropertyMetadata _metadata;

        #endregion

        #region Properties

        public PropertyMetadata Metadata {
            get { return _metadata; }
        }

        #endregion

        #region Construction

        public MissingRequiredOptionException(PropertyMetadata metadata)
            : base("Missing required option: " + metadata.Key) {
            _metadata = metadata;
        }

        #endregion
    }
}