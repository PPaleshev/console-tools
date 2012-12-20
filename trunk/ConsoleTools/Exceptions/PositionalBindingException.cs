using System;


namespace ConsoleTools.Exceptions {
    public class PositionalBindingException : BindingException {
        #region Construction

        public PositionalBindingException(int position) 
            : base("Unable to bind argument at position " + position) {
        }

        #endregion
    }
}