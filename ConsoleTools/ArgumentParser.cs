// ����� "Nogard" ������� [dragon.metalheart@gmail.com]

using System;

namespace ConsoleTools {
    /// <summary>
    /// �����, ����������� ������ ������ ����������, ���������� ��� ������ ����������.
    /// </summary>
    public class ArgumentParser {
        /// <summary>
        /// ������ ��������� �� ���������.
        /// </summary>
        static readonly string[] DefaultPrefixes = new[] {"/", "-", "--"};

        /// <summary>
        /// ������ ������������ ����� � �������� � ����������� ���������� �� ���������.
        /// </summary>
        static readonly char[] DefaultSeparators = new[] {':', '='};

        /// <summary>
        /// ������ ������������ ����� � �������� ��� ������� ����������� ����������.
        /// </summary>
        readonly char[] separators;

        /// <summary>
        /// ������ ��������� ����������� ����������.
        /// </summary>
        readonly string[] prefixes;

        /// <summary>
        /// ������ ����� ��������� ������� � ���������� � ������������� �� ���������.
        /// </summary>
        public ArgumentParser()
        {
            separators = DefaultSeparators;
            prefixes = DefaultPrefixes;
        }

        /// <summary>
        /// ������ ����� ��������� �������.
        /// </summary>
        /// <param name="prefixes">������ ��������� ��� ������� ����������� ����������.</param>
        /// <param name="separators">������ ������������.</param>
        public ArgumentParser(string[] prefixes, char[] separators)
        {
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            if (separators == null || separators.Length == 0)
                throw new ArgumentException("separators");
            this.prefixes = prefixes;
            this.separators = separators;
        }

        /// <summary>
        /// ��������� ������ ���������� ����������.
        /// </summary>
        /// <param name="args">��������� ��� �������.</param>
        /// <returns></returns>
        public CmdArgs Parse(params string[] args) {
            var result = new CmdArgs();

            foreach (var arg in args) {
                if (string.IsNullOrEmpty(arg))
                    continue;

                string argPrefix;
                if (!CheckIsNamedArgument(arg, out argPrefix)) {
                    result.AddUnboundArg(arg);
                    continue;
                }

                var separatorIndex = arg.IndexOfAny(separators, argPrefix.Length);
                var option = arg.Substring(argPrefix.Length, separatorIndex == -1 ? arg.Length - argPrefix.Length : separatorIndex - argPrefix.Length);
                //���� � ��������� �� ������� ������������, �� ��� ����
                var optionValue = separatorIndex == -1 ? string.Empty : arg.Substring(separatorIndex + 1, arg.Length - separatorIndex - 1);
                result.AddNamedArg(option, optionValue);
            }

            return result;
        }

        /// <summary>
        /// ���������, �������� �� ���������� �������� ����������� ���������.
        /// </summary>
        /// <param name="value">����������� ��������.</param>
        /// <param name="prefix">������������ ��������, �������������� ����� �������, ������� ���������� ����������� ���������.</param>
        /// <returns>���������� true, ���� ���������� �������� �������� ����������� ���������, ����� false.</returns>
        bool CheckIsNamedArgument(string value, out string prefix) {
            foreach (string argprefix in prefixes) {
                if (value.StartsWith(argprefix)) {
                    prefix = argprefix;
                    return true;
                }
            }
            prefix = string.Empty;
            return false;
        }
    }
}