using System;


namespace ConsoleTools.Binding {
    /// <summary>
    /// ���� �����. �������� � ���� ������ �������� ����� � ��� ���������.
    /// </summary>
    public struct OptionKey
    {
        /// <summary>
        /// �������� �����. ����������� ��� ����������.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// ��������� �����. ������������ ��� ��������.
        /// </summary>
        public readonly string Alias;

        /// <summary>
        /// ����, ������ true, ���� ������� ���� �������� ������� �������� ��������, ����� false.
        /// </summary>
        public bool HasAlias
        {
            get { return !string.IsNullOrEmpty(Alias); }
        }

        /// <summary>
        /// ������ ����� ��������� ��������� � ��������� �������� ����� � � ����������.
        /// </summary>
        /// <param name="name">�������� �����.</param>
        /// <param name="alias">��������� ����� (������� ������������).</param>
        public OptionKey(string name, string alias)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            Name = name;
            Alias = alias;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ������ ����� ��������� ��������� ��� �������� ��������.
        /// </summary>
        /// <param name="name">������������ �����.</param>
        public OptionKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            Name = name;
            Alias = string.Empty;
        }

        /// <summary>
        /// �������� �������� �������������� ���������� �������� � ������� {name};{alias} � ���� �����.
        /// </summary>
        /// <param name="value">������ � ������� {name}[;{alias}], ��� name - ������������ ��� ��������, � alias - ������������ ���������.</param>
        /// <returns>���������� ����� ��������� ����� �����.</returns>
        public static implicit operator OptionKey(string value) {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            var values = value.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length < 1 || values.Length > 2)
                throw new FormatException("Invalid value format. Expected '<name>[;alias]' format");
            return values.Length == 2 ? new OptionKey(values[0], values[1]) : new OptionKey(values[0]);
        }

        //----------------------------------------------------------------------[]
        public override string ToString() {
            var result = Name;
            if (HasAlias)
                result += ";" + Alias;
            return result;
        }
    }
}