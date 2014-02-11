using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// ��������������� ����� ��� ������ ���������� �� ���������� ����������.
    /// </summary>
    public class UsagePrinter
    {
        /// <summary>
        /// ����, ������ true, ���� ��� ������ ������������� �� ����� �������� ����.
        /// </summary>
        readonly bool showLogo;

        /// <summary>
        /// ������, ��������������� ���������� � ����������.
        /// </summary>
        readonly IApplicationDataProvider appInfo;

        /// <summary>
        /// ��� ������.
        /// </summary>
        readonly Type modelType;

        /// <summary>
        /// ���������� ������� ������.
        /// </summary>
        readonly IList<PropertyMetadata> metadata;

        /// <summary>
        /// ��������� ������� ����������� ����������.
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
        /// ���������� ������ � ��������� ������������� ����������.
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
        /// ����� ���������� �� ������������� ���������� � ��������� <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">������ ��� ������ ���������� �� ������������� ����������.</param>
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
        /// ���������� ���������� � ����������� ���������� (����).
        /// ���������� �������� ��� ����������, ������ � ��������� �����.
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
        /// ����� �������� ����� � �������� ��������.
        /// </summary>
        /// <param name="context">�������� �������� ������.</param>
        /// <param name="text">�����.</param>
        /// <param name="newline">����, ������ true, ���� ����� �������� ������� ������ ����� ������ ������.</param>
        /// <remarks>���� ������ ��� ������ ������, �� ���� <paramref name="newline"/> ������������.</remarks>
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