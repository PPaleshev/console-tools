using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    public class OptionsWithTwoUnboundOptionsProperties {
        #region Properties
 
        [UnboundOptions]
        public string DefaultOption1 {
            get { return null; }
            set {}
        }

        //----------------------------------------------------------------------[]
        [UnboundOptions]
        public string DefaultOption2 {
            get { return null;}
            set{ }
        }
        #endregion
    }
}