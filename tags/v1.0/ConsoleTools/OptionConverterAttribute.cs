using System;


namespace ConsoleTools {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class OptionConverterAttribute : Attribute {
        #region Methods

        public abstract object Convert(string input, Type targetType);

        #endregion
    }
}