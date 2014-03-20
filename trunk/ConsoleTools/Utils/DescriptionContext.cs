using System;
using System.Collections.Generic;
using System.IO;
using ConsoleTools.Binding;
using System.Linq;

namespace ConsoleTools.Utils
{
    /// <summary>
    /// Контекст описания модели.
    /// </summary>
    public class DescriptionContext
    {
        /// <summary>
        /// Список именованных свойств модели.
        /// </summary>
        public IList<PropertyMetadata> NamedProperties { get; private set; }

        /// <summary>
        /// Список позиционных свойств модели, упорядоченных по возрастанию номера позиции.
        /// </summary>
        public IList<PropertyMetadata> PositionalOrderedProperties { get; private set; }

        /// <summary>
        /// Свойство для сборки непривязанных аргументов. Может быть равно <c>null</c>.
        /// </summary>
        public PropertyMetadata UnboundProperty { get; private set; }

        /// <summary>
        /// Флаг, равный true, модель не содержит ни одного свойства.
        /// </summary>
        public bool HasNoArguments { get; private set; }

        /// <summary>
        /// Информация о параметрах разбора именованных аргументов. 
        /// </summary>
        public NamedArgumentsPolicyAttribute NamedArgsInfo { get; private set; }

        /// <summary>
        /// Объект для вывода информации.
        /// </summary>
        public TextWriter Writer { get; private set; }

        /// <summary>
        /// Информация о запускаемом приложении.
        /// </summary>
        public IApplicationDataProvider ApplicationInfo { get; private set; }

        /// <summary>
        /// Создаёт новый контекст описания свойств модели.
        /// </summary>
        public DescriptionContext(IApplicationDataProvider infoProvider, Type modelType, TextWriter writer)
        {
            ApplicationInfo = infoProvider;
            Writer = writer;
            var properties = MetadataProvider.ReadPropertyMetadata(modelType);
            HasNoArguments = properties.Count == 0;
            var data = properties.GroupBy(metadata => metadata.PropertyKind).ToDictionary(g => g.Key, g => g.ToList());
            List<PropertyMetadata> temp;
            if (data.TryGetValue(Kind.Named, out temp))
                NamedProperties = temp;
            if (data.TryGetValue(Kind.Positional, out temp))
                PositionalOrderedProperties = temp.OrderBy(m => m.Specification.Position).ToList();
            if (data.TryGetValue(Kind.Unbound, out temp))
            {
                if (temp.Count > 1)
                    throw new InvalidOperationException("Only one property could be marked by " + typeof(UnboundAttribute).Name);
                if (temp.Count == 1)
                    UnboundProperty = temp[0];
            }
            NamedArgsInfo = MetadataProvider.GetNamedArgumentsPolicy(modelType);
        }

        /// <summary>
        /// Пишет в <see cref="Writer"/> текст <paramref name="text"/>, заключая его соответствующие скобки в зависимости от флага <paramref name="isRequired"/>.
        /// </summary>
        /// <remarks>Вызывается метод <see cref="TextWriter.Write(string)"/>, не выполняется возврат каретки.</remarks>
        public void WriteQuoted(string text, bool isRequired)
        {
            Writer.Write(isRequired ? "<{0}>" : "[{0}]", text);
        }

        /// <summary>
        /// Вставляет <paramref name="count"/> символов перевода строки.
        /// </summary>
        public void WriteLine(uint count = 1)
        {
            for (int i = 0; i < count; i++)
                Writer.WriteLine();
        }

        /// <summary>
        /// Проверяет наличие <see cref="NamedArgsInfo"/>. Если значение отсутствует, выбрасывает исключение.
        /// </summary>
        public void EnsureNamedArgsInfoDefined()
        {
            if (NamedArgsInfo == null)
                throw new InvalidOperationException("To use UsagePrinter you must provide " + typeof(NamedArgumentsPolicyAttribute).Name + " with model to specify prefix and separator");
        }
    }
}