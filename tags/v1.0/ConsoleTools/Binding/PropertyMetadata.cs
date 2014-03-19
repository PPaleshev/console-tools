using System.ComponentModel;

namespace ConsoleTools.Binding {
    /// <summary>
    /// �����, �������������� ����� ����������, ����������� � �������� ���������� ��������.
    /// </summary>
    public class PropertyMetadata {
        /// <summary>
        /// ���� �����.
        /// </summary>
        public PropertyKey Key { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ��������.
        /// </summary>
        public string Meaning { get; set; }

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
        /// ����, ������ true, ���� �������� �������� ��������� ���������, false ��� ��������� ��������.
        /// </summary>
        public bool IsCollection { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� �����.
        /// </summary>
        public string Description {
            get { return PropertyDescriptor.Description; }
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��� ��������, ������������ ������� � ����������.
        /// </summary>
        public Kind PropertyKind { get; set; }

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
        /// ����������� ��������� ���������.
        /// </summary>
        public string CollectionItemSeparator { get; set; }

        /// <summary>
        /// ������ ����� ��������� ���������� �����.
        /// </summary>
        public PropertyMetadata(PropertyDescriptor property)
        {
            PropertyKind = Kind.Unbound;
            PropertyDescriptor = property;
        }
    }
}