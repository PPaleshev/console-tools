using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, для декларативного задания префиксов и разделителей, используемых при разборе именованных аргументов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NamedArgumentsParsingAttribute : Attribute
    {
        /// <summary>
        /// Массив префиксов, которые могут быть использованы для определения именованных аргументов.
        /// </summary>
        public string[] Prefixes { get; private set; }

        /// <summary>
        /// Массив разделителей значений
        /// </summary>
        public string[] Separators { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр атрибута для указания префиксов и разделителей, используемых при разборе аргументов командной строки.
        /// </summary>
        public NamedArgumentsParsingAttribute(string[] prefixes, string[] separators)
        {
            Prefixes = prefixes;
            Separators = separators;
        }
    }
}