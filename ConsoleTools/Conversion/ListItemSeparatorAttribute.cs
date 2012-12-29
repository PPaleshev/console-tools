using System;


namespace ConsoleTools.Conversion {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListItemSeparatorAttribute : Attribute {
        #region Data

        private readonly string _separator = ",";

        #endregion

        public string Separator {
            get { return _separator; }
        }

        #region Construction

        public ListItemSeparatorAttribute(string separator) {
            _separator = separator;
        }

        #endregion
    }
}