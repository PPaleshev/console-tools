using System;
using System.Text;


namespace ConsoleTools.Conversion {
    
    public class EncodingConverterAttribute : OptionConverterAttribute {
        #region Overrides

        public override object Convert(string input, Type targetType) {
            if (targetType != typeof(Encoding))
                throw new NotSupportedException();

            return Encoding.GetEncoding(input);
        }

        #endregion
    }
}