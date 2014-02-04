using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ConsoleTools.Binding;
using ConsoleTools.Exceptions;
using System.Linq;


namespace ConsoleTools {
    /// <summary>
    /// ����� ��� ������������� �������� �������� ���������� ���������� ��������� ������� ��������� ������.
    /// </summary>
    public class OptionsBinder
    {
        /// <summary>
        /// ������-��������� ��� ���������� ����������.
        /// </summary>
        readonly CmdArgs args;
        
        /// <summary>
        /// ������ ���������� ���� ������� ������.
        /// </summary>
        readonly IList<OptionMetadata> optionsMetadata = new List<OptionMetadata>();

        /// <summary>
        /// ������ ������� �������, ��� ������� ������� ����������� �������� ����������� ����������.
        /// </summary>
        readonly HashSet<int> boundFreeArgsPositions = new HashSet<int>();

        /// <summary>
        /// ������ ����� ��������� �������.
        /// </summary>
        /// <param name="args">��������� ���������� ����������.</param>
        OptionsBinder(CmdArgs args)
        {
            this.args = args;
        }

        /// <summary>
        /// ��������� �������� ���������� ���������� <paramref name="args"/> � ��������� ������.
        /// </summary>
        /// <typeparam name="TOptions">��� ������.</typeparam>
        /// <param name="args">���������� ���������.</param>
        public static TOptions BindTo<TOptions>(string[] args) where TOptions : new()
        {
            var parser = new ArgumentParser();
            return BindTo<TOptions>(parser.Parse(args));
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ����������, ����������� � ��������� <paramref name="args"/> � ��������� ������.
        /// </summary>
        /// <typeparam name="TOptions">��� ������.</typeparam>
        /// <param name="args">��������� ����������.</param>
        public static TOptions BindTo<TOptions>(CmdArgs args) where TOptions : new()
        {
            var binder = new OptionsBinder(args);
            return binder.Bind<TOptions>();
        }

        /// <summary>
        /// ��������� �������� ���������� � ��������� ������.
        /// </summary>
        /// <typeparam name="TOptions">��� ������, ���������� ������������� ��������.</typeparam>
        /// <returns>���������� ������ � �������������� ���������� �������.</returns>
        TOptions Bind<TOptions>() where TOptions : new()
        {
            ExtractDeclaredOptionDefinitions(typeof(TOptions));
            var optionsObject = new TOptions();
            BindNamedOptions(optionsObject);
            BindPositionalOptions(optionsObject);
            CollectUnboundOptions(optionsObject);
            return optionsObject;
        }

        /// <summary>
        /// ��������� ���������� �� ������� ������.
        /// </summary>
        /// <param name="optionsType">��� ������.</param>
        void ExtractDeclaredOptionDefinitions(Type optionsType)
        {
            var descriptors = TypeDescriptor.GetProperties(optionsType);
            foreach (PropertyDescriptor property in descriptors)
            {
                var attr = (OptionBindingAttribute)property.Attributes[typeof(OptionBindingAttribute)];
                if (attr == null)
                    continue;
                var metadata = new OptionMetadata(property);
                metadata.IsRequired = attr.IsRequired;
                metadata.IsSwitch = property.Attributes[typeof(SwitchAttribute)] != null;
                attr.FillMetadata(metadata);
                optionsMetadata.Add(metadata);
            }
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ����������� ���������� � ��������� �������.
        /// </summary>
        /// <param name="target">������� ������.</param>
        void BindNamedOptions(object target)
        {
            foreach (var metadata in optionsMetadata.Where(metadata => metadata.OptionType == OptionType.Named))
                BindNamedOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ����������� ���������� � ��������� �������.
        /// </summary>
        /// <param name="target">������� ������.</param>
        void BindPositionalOptions(object target)
        {
            foreach (var metadata in optionsMetadata.Where(metadata => metadata.OptionType == OptionType.Positional))
                BindPositionalOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� ��� ������������� ���������.
        /// </summary>
        /// <param name="target">������� ������.</param>
        void CollectUnboundOptions(object target)
        {
            try
            {
                var unboundOptionsTarget = optionsMetadata.SingleOrDefault(metadata => metadata.OptionType == OptionType.Unbound);
                if (unboundOptionsTarget != null)
                    BindUnboundOptions(unboundOptionsTarget, target);
            }
            catch (InvalidOperationException)
            {
                throw new BindingException("Only one property can contain unbound options");
            }
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ����������� �������� ������������ ��������� � �������� �������� �������.
        /// </summary>
        /// <param name="metadata">���������� ��������.</param>
        /// <param name="target">������� ������.</param>
        void BindNamedOption(OptionMetadata metadata, object target)
        {
            string rawValue = null;
            object convertedValue = null;
            bool requiresConversion = true,
                 needBinding = true;

            string temp;
            if (metadata.IsSwitch)
            {
                requiresConversion = false;
                convertedValue = args.Contains(metadata.Key.Name) || (metadata.Key.HasAlias && args.Contains(metadata.Key.Alias));
            }
            else if (args.TryGetNamedValue(metadata.Key.Name, out temp))
                rawValue = temp;
            else if (metadata.Key.HasAlias && args.TryGetNamedValue(metadata.Key.Alias, out temp))
                rawValue = temp;
            else if (metadata.IsRequired)
                throw new MissingRequiredOptionException(metadata);
            else
            {
                if (metadata.PropertyDescriptor.CanResetValue(target))
                    metadata.PropertyDescriptor.ResetValue(target);
                requiresConversion = false;
                needBinding = false;
            }
            if (requiresConversion)
                convertedValue = ContextfulConvert(rawValue, target, metadata);
            if (needBinding)
                metadata.PropertyDescriptor.SetValue(target, convertedValue);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ����������� �������� ������������ ��������� � �������� �������� �������.
        /// </summary>
        /// <param name="metadata">���������� ��������.</param>
        /// <param name="target">������� ������.</param>
        void BindPositionalOption(OptionMetadata metadata, object target)
        {
            bool requiresConversion = true,
                 requiresBinding = true;
            string rawValue = null;
            object convertedValue = null;

            var descriptor = metadata.PropertyDescriptor;
            if (metadata.Position >= args.UnboundValues.Count)
            {
                //���������� ��������� ������������ ����������� ��������
                if (metadata.IsRequired)
                    throw new PositionalBindingException(metadata.Position);
                //���� �� ��������������, �� ����� ������������ �������� �� ���������
                if (descriptor.CanResetValue(target))
                    descriptor.ResetValue(target);
                requiresBinding = requiresConversion = false;
            }
            else
            {
                rawValue = args.UnboundValues[metadata.Position];
                boundFreeArgsPositions.Add(metadata.Position);
            }
            if (requiresConversion)
                convertedValue = ContextfulConvert(rawValue, target, metadata);
            if (requiresBinding)
                descriptor.SetValue(target, convertedValue);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� ��� ������������� ��������� � ����������� ��������, ���� ����� �������.
        /// </summary>
        /// <param name="metadata">���������� ��������, � ������� ������ ���� �������� ��� ����������� ���������.</param>
        /// <param name="target">������� ������.</param>
        void BindUnboundOptions(OptionMetadata metadata, object target)
        {
            var unboundArgs = new List<string>();
            for (int i = 0; i < args.UnboundValues.Count; i++)
                if (!boundFreeArgsPositions.Contains(i))
                    unboundArgs.Add(args.UnboundValues[i]);
            var descriptor = metadata.PropertyDescriptor;
            if (descriptor.PropertyType.IsArray)
                descriptor.SetValue(target, unboundArgs.ToArray());
            else
            {
                var list = Activator.CreateInstance(descriptor.PropertyType) as IList;
                foreach (string arg in unboundArgs)
                    list.Add(arg);
                descriptor.SetValue(target, list);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="instance"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        static object ContextfulConvert(string text, object instance, OptionMetadata metadata)
        {
            var context = new BindingContext(instance, metadata);
            return metadata.PropertyDescriptor.Converter.ConvertFromString(context, CultureInfo.InvariantCulture, text);
        }
    }
}