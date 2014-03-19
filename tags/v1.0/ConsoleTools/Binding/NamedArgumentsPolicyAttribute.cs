using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, для декларативного указания префикса и разделителя, используемых при разборе именованных аргументов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NamedArgumentsPolicyAttribute : Attribute
    {
        /// <summary>
        /// Префикс, используемый для определения именованных аргументов.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Разделитель названия и значения для именованных аргументов.
        /// </summary>
        public char Separator { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута для указания префикса и разделителя, используемых при разборе именованных аргументов командной строки.
        /// </summary>
        public NamedArgumentsPolicyAttribute(string prefix, char separator)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("prefix");
            if (string.IsNullOrWhiteSpace("separator"))
                throw new ArgumentException("separator");
            Prefix = prefix;
            Separator = separator;
        }
    }
}