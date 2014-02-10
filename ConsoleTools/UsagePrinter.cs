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
            if (showLogo)
                WriteLogo(writer);
            WriteCommandLineExample(writer);
            WriteParametersDescription(writer);
        }

        /// <summary>
        /// ���������� ���������� � ����������� ���������� (����).
        /// ���������� �������� ��� ����������, ������ � ��������� �����.
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
        /// ����� ����� <paramref name="text"/> � <paramref name="writer"/>, ������ ���� �� �� ������.
        /// </summary>
        /// <param name="writer">������ ��� ������ ������.</param>
        /// <param name="text">�����.</param>
        /// <param name="newline">����, ������ true, ���� ����� �������� ������� ������ ����� ������ ������.</param>
        /// <remarks>���� ������ ��� ������ ������, �� ���� <paramref name="newline"/> ������������.</remarks>
        static void WriteText(TextWriter writer, string text, bool newline = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            writer.Write(text);
            if (newline)
                writer.WriteLine();
        }

        /// <summary>
        /// ����� ������ ������� ���������� � �������� �����.
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
        /// ����� �������������� ������� ������������� ����������.
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
        /// ����� �������������� ������� ������������� ����������� ����������.
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
        /// ����� �������������� ������� ������������� ����������� ����������.
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
        /// ����� �������������� ������� ������������� ��������� ����������.
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
        /// ����� �������� ���������� � �������� �����.
        /// </summary>
        void WriteParametersDescription(TextWriter writer)
        {
        }
    }
}