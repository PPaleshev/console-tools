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
    /// ����� ��� �������� �������� ���������� ���������� ��������� ������� ��������� ������.
    /// </summary>
    public class ModelBinder
    {
        /// <summary>
        /// ������-��������� ��� ���������� ����������.
        /// </summary>
        readonly CmdArgs args;
        
        /// <summary>
        /// ������ ������� �������, ��� ������� ������� ����������� �������� ����������� ����������.
        /// </summary>
        readonly HashSet<int> boundFreeArgsPositions = new HashSet<int>();

        /// <summary>
        /// ������ ����� ��������� �������.
        /// </summary>
        /// <param name="args">��������� ���������� ����������.</param>
        ModelBinder(CmdArgs args)
        {
            this.args = args;
        }

        /// <summary>
        /// ��������� �������� ���������� ���������� <paramref name="args"/> � ��������� ������.
        /// </summary>
        /// <typeparam name="TModel">��� ������.</typeparam>
        /// <param name="args">���������� ���������.</param>
        public static TModel BindTo<TModel>(string[] args) where TModel : new()
        {
            var policyAttr = (NamedArgumentsPolicyAttribute)Attribute.GetCustomAttribute(typeof(TModel), typeof(NamedArgumentsPolicyAttribute));
            var parser = policyAttr == null ? new ArgumentParser() : new ArgumentParser(new[] {policyAttr.Prefix}, new[] {policyAttr.Separator});
            return BindTo<TModel>(parser.Parse(args));
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ����������, ����������� � ��������� <paramref name="args"/> � ��������� ������.
        /// </summary>
        /// <typeparam name="TModel">��� ������.</typeparam>
        /// <param name="args">��������� ����������.</param>
        public static TModel BindTo<TModel>(CmdArgs args) where TModel : new()
        {
            var binder = new ModelBinder(args);
            return binder.Bind<TModel>();
        }

        /// <summary>
        /// ��������� �������� ���������� � ��������� ������.
        /// </summary>
        /// <typeparam name="TModel">��� ������, ���������� ������������� ��������.</typeparam>
        /// <returns>���������� ������ � �������������� ���������� �������.</returns>
        TModel Bind<TModel>() where TModel : new()
        {
            var propertyMetadata = MetadataProvider.ReadPropertyMetadata(typeof(TModel));
            var model = new TModel();
            var propertyGroups = propertyMetadata.GroupBy(metadata => metadata.PropertyKind);
            foreach (var propertyGroup in propertyGroups)
            {
                if (propertyGroup.Key == Kind.Named)
                    BindNamedOptions(propertyGroup, model);
                else if (propertyGroup.Key == Kind.Positional)
                    BindPositionalOptions(propertyGroup, model);
                else
                    CollectUnboundOptions(propertyGroup, model);
            }
            return model;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ����������� ���������� � ��������� �������.
        /// </summary>
        /// <param name="properties">������������ ���������� ����������� ������� ������.</param>
        /// <param name="target">������� ������.</param>
        void BindNamedOptions(IEnumerable<PropertyMetadata> properties, object target)
        {
            foreach (var metadata in properties.Where(metadata => metadata.PropertyKind == Kind.Named))
                BindNamedOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��������� �������� ����������� ���������� � ��������� �������.
        /// </summary>
        /// <param name="properties">������������ ���������� ����������� ������� ������.</param>
        /// <param name="target">������� ������.</param>
        void BindPositionalOptions(IEnumerable<PropertyMetadata> properties, object target)
        {
            foreach (var metadata in properties)
                BindPositionalOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� ��� ������������� ���������.
        /// </summary>
        /// <param name="properties">������������ ���������� ����������� ������� ������.</param>
        /// <param name="target">������� ������.</param>
        void CollectUnboundOptions(IEnumerable<PropertyMetadata> properties, object target)
        {
            try
            {
                var unboundOptionsTarget = properties.SingleOrDefault();
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
        void BindNamedOption(PropertyMetadata metadata, object target)
        {
            string rawValue = null;
            object convertedValue = null;
            bool requiresConversion = true,
                 requiresBinding = true;

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
                requiresBinding = false;
            }
            if (requiresConversion)
                convertedValue = ContextfulConvert(rawValue, target, metadata);
            if (requiresBinding)
                metadata.PropertyDescriptor.SetValue(target, convertedValue);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ����������� �������� ������������ ��������� � �������� �������� �������.
        /// </summary>
        /// <param name="metadata">���������� ��������.</param>
        /// <param name="target">������� ������.</param>
        void BindPositionalOption(PropertyMetadata metadata, object target)
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
        void BindUnboundOptions(PropertyMetadata metadata, object target)
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
        /// ��������� �������������� �������� �������� �� ������ � �������� ��� ��������, ��������� ��������� �� <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="text">��������� ��������.</param>
        /// <param name="instance">��������� ������.</param>
        /// <param name="metadata">���������� ��������.</param>
        /// <returns>���������� ��������������� �������� ��������.</returns>
        static object ContextfulConvert(string text, object instance, PropertyMetadata metadata)
        {
            var context = new BindingContext(instance, metadata);
            return metadata.PropertyDescriptor.Converter.ConvertFromString(context, CultureInfo.InvariantCulture, text);
        }
    }
}