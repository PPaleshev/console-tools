using ConsoleTools.Binding;

namespace ConsoleTools.Exceptions {
    /// <summary>
    /// ����������, ����������� ��� ���������� �������� ������������� ���������.
    /// </summary>
    public class MissingRequiredOptionException : BindingException {
        /// <summary>
        /// ���������� ��������, ��� ���������� �������� �������� ����������.
        /// </summary>
        public PropertyMetadata Metadata { get; private set; }

        /// <summary>
        /// ������ ����� ��������� ����������.
        /// </summary>
        /// <param name="metadata">���������� ��������, ��� ���������� �������� �������� ������.</param>
        public MissingRequiredOptionException(PropertyMetadata metadata) : base("Missing required option")
        {
            Metadata = metadata;
        }
    }
}