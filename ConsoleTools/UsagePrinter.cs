using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConsoleTools.Binding;

/*
        Console.WriteLine("\ndefault: <usage statement here>.\n");
        Console.WriteLine("Usage:\n  default [-arg1 <value>] [-arg2]");
 */

namespace ConsoleTools
{
    /// <summary>
    /// Вспомогательный класс для печати информации об аргументах приложения.
    /// </summary>
    public class UsagePrinter
    {
        /// <summary>
        /// Флаг, равный true, если при печати использования не нужно печатать лого.
        /// </summary>
        readonly bool showLogo;

        /// <summary>
        /// Объект, предоставляющий информацию о приложении.
        /// </summary>
        readonly IApplicationDataProvider appInfo;

        /// <summary>
        /// Тип модели.
        /// </summary>
        readonly Type modelType;

        /// <summary>
        /// Метаданные свойств модели.
        /// </summary>
        readonly IList<PropertyMetadata> metadata;

        /// <summary>
        /// Параметры разбора именованных аргументов.
        /// </summary>
        readonly NamedArgumentsPolicyAttribute namedArgumentsPolicy;

        public UsagePrinter(Type modelType, bool showLogo = true)
            : this(modelType, new EntryAssemblyDataProvider(), showLogo)
        {
        }

        public UsagePrinter(Type modelType, IApplicationDataProvider dataProvider, bool showLogo = true)
        {
            this.showLogo = showLogo;
            this.modelType = modelType;
            appInfo = dataProvider;
            metadata = MetadataProvider.ReadPropertyMetadata(modelType);
            namedArgumentsPolicy = MetadataProvider.GetNamedArgumentsPolicy(modelType);
        }

        /// <summary>
        /// Возвращает строку с описанием использования приложения.
        /// </summary>
        public string Print()
        {
            var buffer = new StringBuilder();
            using (var writer = new StringWriter(buffer))
            {
                Print(writer);
                writer.Flush();
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Пишет информацию об использовании приложения в указанный <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">Объект для вывода информации об использовании приложения.</param>
        public void Print(TextWriter writer)
        {
            if (showLogo)
                WriteLogo(writer);
            WriteCommandLineExample(writer);
            WriteParametersDescription(writer);
        }

        /// <summary>
        /// Записывает информацию о запускаемом приложении (лого).
        /// Информация содержит имя приложения, версию и авторские права.
        /// </summary>
        void WriteLogo(TextWriter writer)
        {
            WriteText(writer, appInfo.Title);
            WriteText(writer, " v" + appInfo.Version, true);
            WriteText(writer, appInfo.Description, true);
            WriteText(writer, appInfo.Copyright, true);
            writer.WriteLine();
        }

        /// <summary>
        /// Пишет текст <paramref name="text"/> в <paramref name="writer"/>, только если он не пустой.
        /// </summary>
        /// <param name="writer">Объект для вывода текста.</param>
        /// <param name="text">Текст.</param>
        /// <param name="newline">Флаг, равный true, если нужно добавить перевод строки после записи текста.</param>
        /// <remarks>Если строка для вывода пустая, то флаг <paramref name="newline"/> игнорируется.</remarks>
        static void WriteText(TextWriter writer, string text, bool newline = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            writer.Write(text);
            if (newline)
                writer.WriteLine();
        }

        /// <summary>
        /// Пишет пример запуска приложения в выходной буфер.
        /// </summary>
        void WriteCommandLineExample(TextWriter writer)
        {
            writer.Write("Syntax: ");
            writer.Write(appInfo.ApplicationExeName);
            if (metadata.Count == 0)
                return;
            writer.Write(" ");
            WriteArgumentsSyntax(writer);
        }

        /// <summary>
        /// Пишет синтаксические правила использования аргументов.
        /// </summary>
        void WriteArgumentsSyntax(TextWriter writer)
        {
            var paramGroups = metadata.GroupBy(p => p.PropertyKind).ToDictionary(g => g.Key);
            IGrouping<Kind, PropertyMetadata> propertiesByGroup;
            if (paramGroups.TryGetValue(Kind.Positional, out propertiesByGroup))
                WritePositionalArgumentsSyntax(writer, propertiesByGroup);
            if (paramGroups.TryGetValue(Kind.Named, out propertiesByGroup))
                WriteNamedArgumentsSyntax(writer, propertiesByGroup);
            if (paramGroups.TryGetValue(Kind.Unbound, out propertiesByGroup))
                WriteOtherArgumentsUsage(writer, propertiesByGroup);
        }

        /// <summary>
        /// Пишет синтаксические правила использования позиционных аргументов.
        /// </summary>
        static void WritePositionalArgumentsSyntax(TextWriter writer, IEnumerable<PropertyMetadata> properties)
        {
            foreach (var property in properties.OrderBy(p => p.Position))
            {
                writer.Write(" ");
                var text = property.IsCollection ? property.Meaning + "=item1,item2,item3" : property.Meaning;
                writer.Write(property.IsRequired ? "<{0}>" : "[{0}]", text);
            }
        }

        /// <summary>
        /// Пишет синтаксические правила использования именованных аргументов.
        /// </summary>
        void WriteNamedArgumentsSyntax(TextWriter writer, IEnumerable<PropertyMetadata> properties)
        {
            if (namedArgumentsPolicy == null)
                throw new ArgumentException("To use UsagePrinter you must provide " + typeof(NamedArgumentsPolicyAttribute).Name + " with model to specify prefix and separator");
            foreach (var property in properties)
            {
                writer.Write(" ");
                var value = namedArgumentsPolicy.Prefix + property.Key.Name;
                if (!property.IsSwitch)
                    value += "=" + (property.IsCollection ? "LIST" : "VALUE");
                writer.Write(property.IsRequired ? "<{0}>" : "[{0}]", value);
            }
        }

        /// <summary>
        /// Пишет синтаксические правила использования свободных аргументов.
        /// </summary>
        static void WriteOtherArgumentsUsage(TextWriter writer, IEnumerable<PropertyMetadata> properties)
        {
            var unbound = properties.SingleOrDefault();
            if (unbound == null)
                throw new ArgumentException("Only one property could be marked by UnboundAttribute");
            if (!string.IsNullOrWhiteSpace(unbound.Meaning))
                writer.Write(unbound.IsRequired ? "<{0}>" : "[{0}]", unbound.Meaning);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Пишет описания параметров в выходной буфер.
        /// </summary>
        void WriteParametersDescription(TextWriter writer)
        {
        }
    }
}