using System;

namespace ConsoleTools.Binding {
    /// <summary>
    /// ������� ������� ��� ���� ���������, �������������� ������ ���������� ����������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ModelBindingAttribute : SpecificationAttribute
    {
        /// <summary>
        /// ��������� �������� ������������ ��������.
        /// </summary>
        readonly string meaning;

        /// <summary>
        /// ����, ������ true, ���� �������� �������� ������ ���� ����������� �������, ����� false.
        /// �� ��������� ����� false.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// �������� �� ���������. ������������ ������ ���� �������� �������� ��������������.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// �������� ��������.
        /// </summary>
        public string Description { get; set; }

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
        /// ���������� ��� ���������, ������������ ���������.
        /// </summary>
        public abstract Kind GetPropertyKind();
    }
}