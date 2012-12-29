using System;
using System.Reflection;
using System.Text;
using ConsoleTools.Utils;


namespace ConsoleTools {
    public class UsagePrinter {
        #region Data

        private readonly OptionMetadata[] _metadata;
        private readonly StringBuilder _builder;

        private bool _noLogo;

        #endregion

        #region Construction

        public UsagePrinter(OptionMetadata[] metadata) {
            _metadata = metadata;
            _builder = new StringBuilder();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Dont print logo
        /// </summary>
        public bool NoLogo {
            get { return _noLogo; }
            set { _noLogo = value; }
        }

        #endregion

        #region Methods

        public string Print() {
            StringBuilder buffer = new StringBuilder();

            if (!NoLogo) {
                PrintLogo();
            }

            PrintCommandLineExample();
            PrintParametersDescription();

            return buffer.ToString();
        }

        #endregion

        #region Routines

        private void PrintLogo() {
            Assembly assembly = Assembly.GetEntryAssembly();
            AssemblyTitleAttribute attr = GetAttribute<AssemblyTitleAttribute>(assembly);
            if (attr != null) {
                _builder.AppendLine(attr.Title);
            }

            AssemblyVersionAttribute va = GetAttribute<AssemblyVersionAttribute>(assembly);
            if (va != null) {
                _builder.Append(" version").Append(va.Version);
            }
            _builder.AppendLine();

            AssemblyDescriptionAttribute da = GetAttribute<AssemblyDescriptionAttribute>(assembly);
            if (da != null && !string.IsNullOrEmpty(da.Description)) {
                _builder.AppendLine(da.Description).AppendLine();
            }

            AssemblyCopyrightAttribute ca = GetAttribute<AssemblyCopyrightAttribute>(assembly);
            if (da != null && !string.IsNullOrEmpty(ca.Copyright)) {
                _builder.AppendLine(ca.Copyright);
            }
            _builder.AppendLine();
        }

        //----------------------------------------------------------------------[]
        private void PrintCommandLineExample() {
            _builder.AppendLine("Usage:");
            _builder.Append(Assembly.GetEntryAssembly().FullName)
                    .Append(" ");
    
            foreach (OptionMetadata definition in _metadata) {
                if (definition.IsRequired) {
                    _builder.Append(definition.Key.Name);
                } else {
                    _builder.AppendFormat("[{0}]", definition.Key.Name);
                }
                _builder.Append(" ");
            }
        }

        //----------------------------------------------------------------------[]
        private void PrintParametersDescription() {
            
        }

        //----------------------------------------------------------------------[]
        private static TAttribute GetAttribute<TAttribute>(Assembly asm) where TAttribute : Attribute {
            return (TAttribute) Attribute.GetCustomAttribute(asm, typeof (TAttribute));
        }

        #endregion
    }
}