using System.IO;
using ConsoleTools.Binding;

namespace ConsoleTools.Utils
{
    /// <summary>
    /// Класс для печати подробной информации об аргументах приложения.
    /// </summary>
    class DetailsWriter
    {
        /// <summary>
        /// Контекст описания модели.
        /// </summary>
        readonly DescriptionContext context;

        /// <summary>
        /// Объект для печати текста.
        /// </summary>
        readonly TextWriter writer;

        /// <summary>
        /// Создаёт новый экземпляр класса для печати подробной информации об аргументах приложения.
        /// </summary>
        /// <param name="context">Контекст описания модели.</param>
        public DetailsWriter(DescriptionContext context)
        {
            this.context = context;
            writer = context.Writer;
        }

        /// <summary>
        /// Пишет расширенную информацию о параметрах, с которыми должно запускаться приложение. 
        /// </summary>
        /// <remarks>После завершения печати не выполняет перевод строки.</remarks>
        public void Write()
        {
            writer.WriteLine("Arguments:");
            WritePositionalArgumentsInfo();
            WriteNamedArgumentsInfo();
            WriteOtherArguments();
        }

        /// <summary>
        /// Пишет описание позиционных аргументов.
        /// </summary>
        void WritePositionalArgumentsInfo()
        {
            if (context.PositionalOrderedProperties == null)
                return;
            foreach (var property in context.PositionalOrderedProperties)
            {
                writer.Write(property.Meaning);
                writer.Write("\t(position: {0}", property.Specification.Position);
                writer.Write(property.IsRequired ? ", required" : ", optional");
                writer.WriteLine(")");
                WriteDescription(property);
            }
        }

        /// <summary>
        /// Пишет описание именованных аргументов.
        /// </summary>
        void WriteNamedArgumentsInfo()
        {
            if (context.NamedProperties == null)
                return;
            context.EnsureNamedArgsInfoDefined();
            var arg = context.NamedArgsInfo;
            foreach (var property in context.NamedProperties)
            {
                var spec = property.Specification;
                writer.Write(arg.Prefix);
                writer.Write(spec.Key.Name);
                if (spec.Key.HasAlias)
                {
                    writer.Write(", ");
                    writer.Write(arg.Prefix);
                    writer.Write(spec.Key.Alias);
                }
                writer.Write(spec.IsSwitch ? "\t(Switch" : "\t(Named");
                writer.Write(property.IsRequired ? ", required" : ", optional");
                writer.WriteLine(")");
                WriteDescription(property);
            }
        }

        /// <summary>
        /// Пишет описание прочих аргументов.
        /// </summary>
        void WriteOtherArguments()
        {
            var prop = context.UnboundProperty;
            if (prop == null || string.IsNullOrWhiteSpace(prop.Meaning))
                return;
            writer.WriteLine(prop.Meaning);
            WriteDescription(prop);
        }

        /// <summary>
        /// Пишет описание свойства.
        /// </summary>
        /// <param name="property">Метаданные свойства.</param>
        void WriteDescription(PropertyMetadata property)
        {
            context.Writer.Write("\t");
            context.Writer.WriteLine(string.IsNullOrWhiteSpace(property.Description) ? "{no description}" : property.Description);
            if (property.DefaultValue != null)
                context.Writer.WriteLine("\tDefault value: {0}", property.DefaultValue);
            context.Writer.WriteLine();
        }
    }
}