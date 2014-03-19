using System;


namespace ConsoleTools.Exceptions {
    public class BindingException : ApplicationException {
        #region Construction

        public BindingException(string message) : base(message) {
        }

        //----------------------------------------------------------------------[]
        public BindingException(string message, Exception innerException)
            : base(message, innerException) {
        }

        #endregion
    }
}