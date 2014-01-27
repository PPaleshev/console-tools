using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleTools {

    /// <summary>
    /// ������������� ������������� ����������� ���������� ��������� ������.
    /// </summary>
    public class CmdArgs
    {
        /// <summary>
        /// ������� ����������� ����������.
        /// ����������� ������������ �������� ��������� � ��� ��������.
        /// </summary>
        readonly IDictionary<string, string> namedArgs;

        /// <summary>
        /// ������ ������������� ���������� � ������� �� ��������� � ������ ����������.
        /// </summary>
        readonly IList<string> unboundArgs = new List<string>();

        /// <summary>
        /// ��������� ������������� ���������� � ������� �� ��������� � ������ ����������.
        /// </summary>
        public ReadOnlyCollection<string> Args {
            get { return new ReadOnlyCollection<string>(unboundArgs); }
        }

        /// <summary>
        /// ������ ����� ��������� ������ ��� ������������� ����������� ����������.
        /// </summary>
        /// <param name="namedArgs">����������� �� ����� ��������� � ��� ��������.</param>
        /// <param name="unboundArgs">������ ������������� ����������.</param>
        public CmdArgs(IDictionary<string, string> namedArgs, IList<string> unboundArgs)
        {
            this.namedArgs = namedArgs ?? new Dictionary<string, string>();
            this.unboundArgs = unboundArgs ?? new ReadOnlyCollection<string>(new string[0]);
        }

        /// <summary>
        /// ������ ������ ��������� ��� ������������� ����������� ����������.
        /// </summary>
        public CmdArgs() {
        }

        /// <summary>
        /// ��������� �������� ������������ ���������.
        /// </summary>
        /// <param name="name">�������� ���������.</param>
        /// <param name="value">�������� ���������.</param>
        public void AddNamedArg(string name, string value) {
            namedArgs[name] = value;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� ����������� ��������.
        /// </summary>
        /// <param name="arg">�������� ������������ ���������.</param>
        public void AddUnboundArg(string arg) {
            unboundArgs.Add(arg);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ������� ������ ����������.
        /// </summary>
        public void Clear() {
            namedArgs.Clear();
            unboundArgs.Clear();
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ����������, ���� �� �������� � ��������� ������ � ������ ����������� ����������.
        /// </summary>
        /// <param name="name">��� ���������.</param>
        /// <returns>���������� true, ���� �������� ���� ����� ����������� ����������, ����� false.</returns>
        public bool Contains(string name) {
            return namedArgs.ContainsKey(name);
        }

        /// <summary>
        /// �������� ������� �������� �� ��� �����.
        /// </summary>
        /// <param name="name">��� ���������.</param>
        /// <param name="value">�������� ���������.</param>
        /// <returns>���������� true, ���� �������� ��������� � ��������� ������ ����������, ����� false.</returns>
        public bool TryGetNamedValue(string name, out string value)
        {
            return namedArgs.TryGetValue(name, out value);
        }
    }
}