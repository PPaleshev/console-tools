using System;
using System.IO;
using System.Reflection;
using System.Text;
using ConsoleTools.Binding;

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
        /// ��������� ����� ��� ������ ����������.
        /// </summary>
        readonly StringBuilder builder;

        /// <summary>
        /// ������, � ������� ���������� ����� ����� � ����������.
        /// </summary>
        readonly Assembly entryAssembly;

        /// <summary>
        /// ��� ������.
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
        /// ���������� ������ � ��������� ������������� ����������.
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
        /// ����� ���������� �� ������������� ���������� � ��������� <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">������ ��� ������ ���������� �� ������������� ����������.</param>
        public void Print(TextWriter writer)
        {
            writer.Write(Print());
            writer.Flush();
        }

        /// <summary>
        /// ���������� ���������� � ����������� ���������� (����).
        /// ���������� �������� ��� ����������, ������ � ��������� �����.
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
        /// ���������� ������ �� ����������� ������, ���������� ����� ����� � ����������.
        /// ������������ ��� ������������.
        /// </summary>
        protected virtual Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// ����� ������ ������� ���������� � �������� �����.
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
        /// ����� �������� ���������� � �������� �����.
        /// </summary>
        void WriteParametersDescription()
        {

        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ��������� �������, ����������� �� ������ ������, �� ��� ����.
        /// </summary>
        /// <typeparam name="TAttribute">��� ��������.</typeparam>
        /// <param name="asm">������, �� ���������� ������� ���������� ������� �</param>
        /// <returns></returns>
        static TAttribute GetAttribute<TAttribute>(Assembly asm) where TAttribute : Attribute
        {
            return (TAttribute)Attribute.GetCustomAttribute(asm, typeof(TAttribute));
        }
    }
}