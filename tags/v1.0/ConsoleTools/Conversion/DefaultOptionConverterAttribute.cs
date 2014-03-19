using System;
using System.Globalization;


namespace ConsoleTools.Conversion {
    public class DefaultOptionConverterAttribute : OptionConverterAttribute {
        #region Methods

        public static readonly DefaultOptionConverterAttribute Instance = new DefaultOptionConverterAttribute();
        #endregion

        #region Overrides of OptionConverterAttribute

        public override object Convert(string input, Type targetType) {
            Type nullableUnderlyingType = Nullable.GetUnderlyingType(targetType);
            return System.Convert.ChangeType(input, nullableUnderlyingType ?? targetType, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}