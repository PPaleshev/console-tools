using System;


namespace ConsoleTools.Exceptions
{
    /// <summary>
    /// ����������, ����������� ��� ������� ����������.
    /// </summary>
    public class BindingException : ApplicationException
    {
        /// <summary>
        /// ������ ����� ��������� ���������� � ��������� ������� ������.
        /// </summary>
        /// <param name="message">�����, ����������� ������� ������.</param>
        public BindingException(string message) : base(message)
        {
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ������ ����� ��������� ����������.
        /// </summary>
        /// <param name="message">�����, ����������� ������� ������.</param>
        /// <param name="innerException">������ ���������� ����������.</param>
        public BindingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}