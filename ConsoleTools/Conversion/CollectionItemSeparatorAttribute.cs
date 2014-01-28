using System;


namespace ConsoleTools.Conversion {
    /// <summary>
    /// ������� ��� �������� ����������� ��������� ���������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionItemSeparatorAttribute : Attribute
    {
        /// <summary>
        /// ����������� ��������� ���������, ������������ ��� �������������� ��������� ����������.
        /// </summary>
        public string Separator { get; private set; }

        /// <summary>
        /// ������ ����� ��������� �������� � ��������� �������� �����������.
        /// </summary>
        /// <param name="separator">����������� ��������� ���������.</param>
        public CollectionItemSeparatorAttribute(string separator)
        {
            Separator = string.IsNullOrEmpty(separator) ? "," : separator;
        }
    }
}