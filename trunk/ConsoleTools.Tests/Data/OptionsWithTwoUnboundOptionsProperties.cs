using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    public class OptionsWithTwoUnboundOptionsProperties {
        #region Properties
 
        [Unbound]
        public string DefaultOption1 {
            get { return null; }
            set {}
        }

        //----------------------------------------------------------------------[]
        [Unbound]
        public string DefaultOption2 {
            get { return null;}
            set{ }
        }
        #endregion
    }
}