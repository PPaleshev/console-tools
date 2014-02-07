using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// ������� ������� ��� ���� ���������, �������������� ������ ���������� ����������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ModelBindingAttribute : Attribute
    {
        /// <summary>
        /// ����, ������ true, ���� �������� ����������� ��� ����������.
        /// </summary>
        readonly bool isRequired;

        /// <summary>
        /// ����, ������ true, ���� �������� �������� ������ ���� ����������� �������, ����� false.
        /// </summary>
        public bool IsRequired
        {
            get { return isRequired; }
        }


        /// <summary>
        /// ������ ����� ��������� ��������.
        /// </summary>
        /// <param name="isRequired">����, ������ true, ���� �������� �������� ������ ���� ����������� �������, ����� false.</param>
        protected ModelBindingAttribute(bool isRequired)
        {
            this.isRequired = isRequired;
        }

        /// <summary>
        /// ������������� � ��������, ��������� ���������� ������������ �����������.
        /// </summary>
        /// <param name="metadata">���������� ��������.</param>
        public abstract void FillMetadata(PropertyMetadata metadata);
    }
}