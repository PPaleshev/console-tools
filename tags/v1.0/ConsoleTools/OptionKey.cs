using System;


namespace ConsoleTools {
    /// <summary>
    /// Ключ опции. Включает в себя полное и сокращённое имя опции
    /// </summary>
    public struct OptionKey {
        #region Properties

        public readonly string Name,
                               Alias;

        #endregion

        #region Properties

        public bool HasAlias {
            get { return !string.IsNullOrEmpty(Alias); }
        }

        #endregion

        #region Construction

        public OptionKey(string name, string alias) {
            Name = name;
            Alias = alias;
        }

        //----------------------------------------------------------------------[]
        public OptionKey(string name) : this() {
            Name = name;
            Alias = string.Empty;
        }

        #endregion

        #region Overrides

        public static implicit operator OptionKey(string value) {
            if (string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException("value");
            }

            string[] values = value.Split(new string[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length <1 || values.Length > 2) {
                throw new FormatException("Invalid value format. Expected 'name' or 'name;alias' formats");
            }

            return values.Length == 2 ? new OptionKey(values[0], values[1]) : new OptionKey(values[0]);
        }

        //----------------------------------------------------------------------[]
        public override string ToString() {
            string result = Name;
            if (HasAlias) {
                result += ";" + Alias;
            }
            return result;
        }

        #endregion
    }
}