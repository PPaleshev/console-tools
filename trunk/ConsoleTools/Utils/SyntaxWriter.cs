using System.IO;
using ConsoleTools.Binding;

namespace ConsoleTools.Utils
{
    /// <summary>
    /// Вспомогательный класс для вывода синтаксиса запуска приложения с аргументами.
    /// </summary>
    class SyntaxWriter
    {
        /// <summary>
        /// Контекст описания модели.
        /// </summary>
        readonly DescriptionContext context;

        /// <summary>
        /// Объект для вывода информации.
        /// </summary>
        readonly TextWriter writer;

        /// <summary>
        /// Создаёт новый вспомогательный объект для печати синтаксиса использования приложения.
        /// </summary>
        /// <param name="context">Контекст описания модели.</param>
        public SyntaxWriter(DescriptionContext context)
        {
            this.context = context;
            writer = context.Writer;
        }

        /// <summary>
        /// Пишет пример запуска приложения.
        /// </summary>
        /// <remarks>После завершения печати не выполняет перевод строки.</remarks>
        public void Write()
        {
            writer.Write("Syntax: ");
            writer.Write(context.ApplicationInfo.ApplicationExeName);
            if (context.HasNoArguments)
                return;
            writer.Write(" ");
            WritePositionalArgumentsSyntax();
            WriteNamedArgumentsSyntax();
            WriteOtherArgumentsUsage();
        }

        /// <summary>
        /// Пишет синтаксические правила использования позиционных аргументов.
        /// </summary>
        void WritePositionalArgumentsSyntax()
        {
            if (context.PositionalOrderedProperties == null)
                return;
            foreach (var property in context.PositionalOrderedProperties)
            {
                writer.Write(" ");
                var text = property.IsCollection ? GetCollectionSpec(property) : property.Meaning;
                context.WriteQuoted(text, property.IsRequired);
            }
        }

        /// <summary>
        /// Пишет синтаксические правила использования именованных аргументов.
        /// </summary>
        void WriteNamedArgumentsSyntax()
        {
            if (context.NamedProperties == null)
                return;
            context.EnsureNamedArgsInfoDefined();
            var arg = context.NamedArgsInfo;
            foreach (var property in context.NamedProperties)
            {
                writer.Write(" ");
                var text = arg.Prefix + property.Key.Name;
                if (property.IsSwitch && property.Key.HasAlias)
                    text += "|" + arg.Prefix + property.Key.Alias;
                if (!property.IsSwitch) // TODO: add code to write named arguments
                    text += arg.Separator + (property.IsCollection ? GetCollectionSpec(property) : "VALUE");
                context.WriteQuoted(text, property.IsRequired);
            }
        }

        /// <summary>
        /// Пишет синтаксические правила использования свободных аргументов.
        /// </summary>
        void WriteOtherArgumentsUsage()
        {
            var property = context.UnboundProperty;
            if (property != null && !string.IsNullOrWhiteSpace(property.Meaning))
                context.WriteQuoted(property.Meaning, property.IsRequired);
        }

        /// <summary>
        /// Возвращает спецификацию для свойства, содержащего коллекцию элементов.
        /// </summary>
        /// <param name="property">Метаданные свойства.</param>
        static string GetCollectionSpec(PropertyMetadata property)
        {
            return string.IsNullOrWhiteSpace(property.Meaning) ? string.Format("item[{0}item{0}...]", property.CollectionItemSeparator) :
                string.Format("{{{1}}}item[{0}item{0}...]", property.CollectionItemSeparator, property.Meaning);
        }
    }
}