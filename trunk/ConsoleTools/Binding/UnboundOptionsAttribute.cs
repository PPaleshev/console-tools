using ConsoleTools.Utils;


namespace ConsoleTools.Binding {
    public class UnboundOptionsAttribute : OptionBindingAttribute {
        #region Construction

        public UnboundOptionsAttribute(bool isRequired) : base(isRequired) {
        }

        //----------------------------------------------------------------------[]
        public UnboundOptionsAttribute() : base(false) {
        }

        #endregion

        #region Overrides

        public override void FillMetadata(OptionMetadata metadata) {
            metadata.ArgumentType = ArgumentType.Unbound;
        }

        #endregion
    }
}