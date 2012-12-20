using System;
using System.Reflection;
using System.Text;


namespace ConsoleTools {
    public class UsagePrinter {
        #region Data

        private readonly OptionMetadata[] _options;

        #endregion

        #region Construction

        public UsagePrinter(OptionMetadata[] options) {
            _options = options;
        }

        #endregion

        #region Methods

        public string Print() {
            StringBuilder buffer = new StringBuilder();
         
            PrintCommandLineExample(buffer);
            PrintParametersDescription(buffer);

            return buffer.ToString();
        }

        #endregion

        #region Routines

        private void PrintCommandLineExample(StringBuilder buffer) {
            buffer.AppendLine("Usage:");
            buffer.Append(Assembly.GetEntryAssembly().FullName)
                .Append(" ");
            foreach (OptionMetadata definition in _options) {
                if (definition.IsRequired)
                    buffer.Append(definition.Key.Name);
                else {
                    buffer.AppendFormat("[{0}]", definition.Key.Name);
                }
                buffer.Append(" ");
            }
        }

        //----------------------------------------------------------------------[]
        private void PrintParametersDescription(StringBuilder buffer) {
            
        }
        #endregion

    }
}