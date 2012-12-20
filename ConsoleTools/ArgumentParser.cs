// Павел "Nogard" Палешев [dragon.metalheart@gmail.com]

namespace ConsoleTools {
    public class ArgumentParser {
        #region Data

        //===============================================================================================[]

        private string[] _argumentPrefixes = new string[] {"/", "-", "--"};
        private char[] _separators = new char[] {':', '='};

        //===============================================================================================[]

        #endregion

        #region Properties

        public char[] Separators {
            get { return _separators; }
            set { _separators = value; }
        }

        //----------------------------------------------------------------------[]
        public string[] ArgumentPrefixes {
            get { return _argumentPrefixes; }
            set { _argumentPrefixes = value; }
        }

        #endregion

        #region Construction

        //===============================================================================================[]
        public ArgumentParser() {
        }

        //----------------------------------------------------------------------[]
        public ArgumentParser(string[] propertyArgumentPrefixes) {
            _argumentPrefixes = propertyArgumentPrefixes;
        }

        //----------------------------------------------------------------------[]
        public ArgumentParser(string[] argumentPrefixes, char[] valueSeparators) {
            _argumentPrefixes = argumentPrefixes;
            _separators = valueSeparators;
        }

        //===============================================================================================[]

        #endregion

        #region Methods

        //===============================================================================================[]
        public CmdArgs Parse(string[] args) {
            CmdArgs result = new CmdArgs();

            foreach (string arg in args) {
                if (string.IsNullOrEmpty(arg)) {
                    continue;
                }

                //Парсинг именованных аргументов. Именованные аргументы начинаются с одного из _argumentPrefix
                string argPrefix;
                if (!TestIsArgument(arg, out argPrefix)) {
                    result.AddDefaultArg(arg);
                    continue;
                }

                int separatorIndex = arg.IndexOfAny(_separators, argPrefix.Length);
                string option = arg.Substring(argPrefix.Length, separatorIndex == -1
                                                                    ? arg.Length - argPrefix.Length
                                                                    : separatorIndex - argPrefix.Length);
                //Если в аргументе не найдено разделителей, то это флаг
                string optionValue = separatorIndex == -1
                                         ? string.Empty
                                         : arg.Substring(separatorIndex + 1, arg.Length - separatorIndex - 1);

                result.AddNamedArg(option, optionValue);
            }

            return result;
        }


        //===============================================================================================[]

        #endregion

        #region Routines

        private bool TestIsArgument(string value, out string prefix) {
            foreach (string argprefix in _argumentPrefixes) {
                if (value.StartsWith(argprefix)) {
                    prefix = argprefix;
                    return true;
                }
            }
            prefix = string.Empty;
            return false;
        }

        //----------------------------------------------------------------------[]
        private bool KeyValueMode {
            get { return _argumentPrefixes.Length == 0; }
        }

        #endregion
    }
}