using System;
using System.Diagnostics;
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
        /// Массив метаданных свойств.
        /// </summary>
        readonly OptionMetadata[] metadata;

        /// <summary>
        /// Флаг, равный true, если при печати использования не нужно печатать лого.
        /// </summary>
        readonly bool noLogo;

        /// <summary>
        /// Строковый буфер для записи результата.
        /// </summary>
        readonly StringBuilder builder;


        public UsagePrinter(OptionMetadata[] metadata, bool noLogo)
        {
            this.metadata = metadata;
            this.noLogo = noLogo;
            builder = new StringBuilder();
        }

        public string Print()
        {
            StringBuilder buffer = new StringBuilder();

            if (!noLogo)
            {
                WriteLogo();
            }

            WriteCommandLineExample();
            WriteParametersDescription();

            return buffer.ToString();
        }

        #region Routines

        void WriteLogo()
        {
            var assembly = Assembly.GetEntryAssembly();
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

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Пишет пример использования приложения в выходной буфер.
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

        #endregion
    }
}