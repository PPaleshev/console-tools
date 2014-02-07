using System;
using System.IO;
using System.Reflection;
using System.Text;
using ConsoleTools.Binding;

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
        /// Строковый буфер для записи результата.
        /// </summary>
        readonly StringBuilder builder;

        /// <summary>
        /// Сборка, в которой содержится точка входа в приложение.
        /// </summary>
        readonly Assembly entryAssembly;

        /// <summary>
        /// Тип модели.
        /// </summary>
        readonly Type modelType;

        public UsagePrinter(Type modelType, bool showLogo = true) : this(Assembly.GetEntryAssembly(), modelType, showLogo)
        {
        }

        public UsagePrinter(Assembly entryAssembly, Type modelType, bool showLogo = true)
        {
            this.showLogo = showLogo;
            builder = new StringBuilder();
            this.modelType = modelType;
            this.entryAssembly = entryAssembly;
        }

        /// <summary>
        /// Возвращает строку с описанием использования приложения.
        /// </summary>
        public string Print()
        {
            var buffer = new StringBuilder();
            if (!showLogo)
                WriteLogo();
            WriteCommandLineExample();
            WriteParametersDescription();
            return buffer.ToString();
        }

        /// <summary>
        /// Пишет информацию об использовании приложения в указанный <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">Объект для вывода информации об использовании приложения.</param>
        public void Print(TextWriter writer)
        {
            writer.Write(Print());
            writer.Flush();
        }

        /// <summary>
        /// Записывает информацию о запускаемом приложении (лого).
        /// Информация содержит имя приложения, версию и авторские права.
        /// </summary>
        void WriteLogo()
        {
            var assembly = GetEntryAssembly();
            var attr = GetAttribute<AssemblyTitleAttribute>(assembly);
            if (attr != null)
                builder.AppendLine(attr.Title);
            var va = GetAttribute<AssemblyVersionAttribute>(assembly);
            if (va != null)
                builder.Append(" v").Append(va.Version);
            builder.AppendLine();
            var da = GetAttribute<AssemblyDescriptionAttribute>(assembly);
            if (da != null && !string.IsNullOrEmpty(da.Description))
                builder.AppendLine(da.Description).AppendLine();
            var ca = GetAttribute<AssemblyCopyrightAttribute>(assembly);
            if (da != null && !string.IsNullOrEmpty(ca.Copyright))
                builder.AppendLine(ca.Copyright);
            builder.AppendLine();
        }

        /// <summary>
        /// Возвращает ссылку на запускаемую сборку, содержащую точку входа в приложение.
        /// Используется для тестирования.
        /// </summary>
        protected virtual Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// Пишет пример запуска приложения в выходной буфер.
        /// </summary>
        void WriteCommandLineExample()
        {
            builder.AppendLine("Usage: ");
            builder.Append(Assembly.GetEntryAssembly().FullName).Append(" ");
            foreach (var definition in metadata)
            {
                if (definition.IsRequired)
                    builder.Append(definition.Key.Name);
                else
                    builder.AppendFormat("[{0}]", definition.Key.Name);
                builder.Append(" ");
            }
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Пишет описания параметров в выходной буфер.
        /// </summary>
        void WriteParametersDescription()
        {

        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Возвращает указанный атрибут, объявленный на уровне сборки, по его типу.
        /// </summary>
        /// <typeparam name="TAttribute">Тип атрибута.</typeparam>
        /// <param name="asm">Сборка, из метаданных которой необходимо извлечь а</param>
        /// <returns></returns>
        static TAttribute GetAttribute<TAttribute>(Assembly asm) where TAttribute : Attribute
        {
            return (TAttribute)Attribute.GetCustomAttribute(asm, typeof(TAttribute));
        }
    }
}