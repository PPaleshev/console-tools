using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ConsoleTools {
    public class CmdArgs {
        #region Data

        private readonly IDictionary<string, string> _namedArgs = new Dictionary<string, string>();
        private readonly IList<string> _defaultArgs = new List<string>();

        #endregion

        #region Properties

        public ReadOnlyCollection<string> Args {
            get { return new ReadOnlyCollection<string>(_defaultArgs); }
        }

        //----------------------------------------------------------------------[]
        public string this[string name] {
            get { return _namedArgs[name]; }
        }

        #endregion

        #region Construction

        public CmdArgs(IDictionary<string, string> namedArgs, IList<string> defaultArgs) {
            _namedArgs = namedArgs ?? _namedArgs;
            _defaultArgs = defaultArgs ?? _defaultArgs;
        }

        //----------------------------------------------------------------------[]
        public CmdArgs() {
        }

        #endregion

        #region Methods

        public void AddNamedArg(string name, string value) {
            _namedArgs[name] = value;
        }

        //----------------------------------------------------------------------[]
        public void AddDefaultArg(string arg) {
            _defaultArgs.Add(arg);
        }

        //----------------------------------------------------------------------[]
        public void Clear() {
            _namedArgs.Clear();
            _defaultArgs.Clear();
        }

        //----------------------------------------------------------------------[]
        public bool Contains(string name) {
            return _namedArgs.ContainsKey(name);
        }

        #endregion
    }
}