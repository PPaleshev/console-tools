using System.ComponentModel;

namespace ConsoleTools.Binding {
    /// <summary>
    /// �����, �������������� ����� ����������, ����������� � �������� ���������� ��������.
    /// </summary>
    public class OptionMetadata {
        /// <summary>
        /// ���� �����.
        /// </summary>
        public OptionKey Key { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ����, ������ true, ���� �������� ����� ����������� ��� ��������, ����� false.
        /// </summary>
        public bool IsRequired { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ����, ������ true, ���� �������� ����� �������� "��������������" ��� ������.
        /// </summary>
        public bool IsSwitch { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� �����.
        /// </summary>
        public string Description {
            get { return PropertyDescriptor.Description; }
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��� �����, ������������ ������� � ����������.
        /// </summary>
        public OptionType OptionType { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ����� �����, ������������ ��� ���������� ����������� ����������.
        /// </summary>
        public int Position { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ��������.
        /// </summary>
        public PropertyDescriptor PropertyDescriptor { get; private set; }

        /// <summary>
        /// ������ ����� ��������� ���������� �����.
        /// </summary>
        public OptionMetadata(PropertyDescriptor property)
        {
            OptionType = OptionType.Unbound;
            PropertyDescriptor = property;
        }
    }
}