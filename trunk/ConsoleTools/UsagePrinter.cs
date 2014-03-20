using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ConsoleTools.Binding;
using ConsoleTools.Utils;

/*
        Console.WriteLine("\ndefault: <usage statement here>.\n");
        Console.WriteLine("Usage:\n  default [-arg1 <value>] [-arg2]");
        <item[,item,...]>
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
        /// Создаёт новый экземпляр класса.
        /// Информация о приложении берётся из метаданных сборки, содержащей точку входа (<see cref="Assembly.GetEntryAssembly"/>).
        /// </summary>
        /// <param name="modelType">Тип модели.</param>
        /// <param name="showLogo">Флаг, определяющий необходимость отображения информации о приложении.</param>
        public UsagePrinter(Type modelType, bool showLogo = true) : this(modelType, new EntryAssemblyDataProvider(), showLogo)
        {
        }

        /// <summary>
        /// Создаёт новый экземпляр класса.
        /// </summary>
        /// <param name="dataProvider">Поставщик основной информации о запускаемом приложении.</param>
        /// <param name="modelType">Тип модели.</param>
        /// <param name="showLogo">Флаг, определяющий необходимость отображения информации о приложении.</param>
        public UsagePrinter(Type modelType, IApplicationDataProvider dataProvider, bool showLogo = true)
        {
            this.showLogo = showLogo;
            this.modelType = modelType;
            appInfo = dataProvider;
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
            var context = new DescriptionContext(appInfo, modelType, writer);
            if (showLogo)
                WriteLogo(context);
            new SyntaxWriter(context).Write();
            context.WriteLine(2);
            new DetailsWriter(context).Write();
        }

        /// <summary>
        /// Записывает информацию о запускаемом приложении (лого).
        /// Информация содержит имя приложения, версию и авторские права.
        /// </summary>
        void WriteLogo(DescriptionContext context)
        {
            WriteText(context, appInfo.Title);
            WriteText(context, " v" + appInfo.Version, true);
            WriteText(context, appInfo.Description, true);
            WriteText(context, appInfo.Copyright, true);
            context.WriteLine();
        }

        /// <summary>
        /// Пишет непустой текст в контекст описания.
        /// </summary>
        /// <param name="context">Контекст описания модели.</param>
        /// <param name="text">Текст.</param>
        /// <param name="newline">Флаг, равный true, если нужно добавить перевод строки после записи текста.</param>
        /// <remarks>Если строка для вывода пустая, то флаг <paramref name="newline"/> игнорируется.</remarks>
        static void WriteText(DescriptionContext context, string text, bool newline = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            context.Writer.Write(text);
            if (newline)
                context.WriteLine();
        }
    }
}