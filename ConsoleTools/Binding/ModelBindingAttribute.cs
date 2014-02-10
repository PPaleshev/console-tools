using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// ������� ������� ��� ���� ���������, �������������� ������ ���������� ����������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ModelBindingAttribute : Attribute
    {
        /// <summary>
        /// �������� ������������ ��������.
        /// </summary>
        readonly string meaning;

        /// <summary>
        /// ����, ������ true, ���� �������� �������� ������ ���� ����������� �������, ����� false.
        /// �� ��������� ����� false.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// �������� ������������ ��������.
        /// </summary>
        public string Meaning
        {
            get { return meaning; }
        }

        /// <summary>
        /// ������ ����� ��������� ��������.
        /// </summary>
        /// <param name="meaning">�������� ������������ ��������.</param>
        protected ModelBindingAttribute(string meaning)
        {
            this.meaning = meaning;
        }

        /// <summary>
        /// ������������� � ��������, ��������� ���������� ������������ �����������.
        /// </summary>
        /// <param name="metadata">���������� ��������.</param>
        public abstract void FillMetadata(PropertyMetadata metadata);
    }
}