using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ConsoleTools.Binding;
using ConsoleTools.Exceptions;
using ConsoleTools.Utils;
using System.Linq;


namespace ConsoleTools {
    public class OptionsBinder
    {
        readonly CmdArgs args;
        readonly IList<OptionMetadata> optionsMetadata = new List<OptionMetadata>();
        readonly List<int> boundFreeArgsPositions = new List<int>();

        OptionsBinder(CmdArgs args)
        {
            this.args = args;
        }

        TOptions Bind<TOptions>() where TOptions : new()
        {
            ExtractDeclaredOptionDefinitions(typeof (TOptions));

            var optionsObject = new TOptions();

            BindNamedOptions(optionsObject);
            BindPositionalOptions(optionsObject);
            BindUnboundOptions(optionsObject);

            return optionsObject;
        }

        void ExtractDeclaredOptionDefinitions(Type optionsType)
        {
            var descriptors = TypeDescriptor.GetProperties(optionsType);
            foreach (PropertyDescriptor property in descriptors)
            {
                var attr = (OptionBindingAttribute) property.Attributes[typeof (OptionBindingAttribute)];
                if (attr == null)
                    continue;
                var metadata = new OptionMetadata(property);
                metadata.IsRequired = attr.IsRequired;
                metadata.IsFlag = property.Attributes[typeof (SwitchAttribute)] != null;
                attr.FillMetadata(metadata);
                optionsMetadata.Add(metadata);
            }
        }

        //----------------------------------------------------------------------[]
        void BindNamedOptions(object target)
        {
            foreach (var metadata in optionsMetadata.Where(metadata => metadata.ArgumentType == ArgumentType.Named))
                BindNamedOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        void BindPositionalOptions(object target)
        {
            foreach (var metadata in optionsMetadata.Where(metadata => metadata.ArgumentType == ArgumentType.Positional))
                BindPositionalOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        void BindUnboundOptions(object target)
        {
            try
            {
                var unboundOptionsTarget = optionsMetadata.SingleOrDefault(metadata => metadata.ArgumentType == ArgumentType.Unbound);
                if (unboundOptionsTarget != null)
                    BindUnboundOptions(unboundOptionsTarget, target);
            }
            catch (InvalidOperationException)
            {
                throw new BindingException("Only one property can contain unbound options");
            }
        }

        //----------------------------------------------------------------------[]
        void BindNamedOption(OptionMetadata metadata, object target)
        {
            string rawValue = null;
            object convertedValue = null;
            bool requiresConversion = true,
                 needBinding = true;

            string temp;
            if (metadata.IsFlag)
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
        void BindPositionalOption(OptionMetadata metadata, object target)
        {
            bool requiresConversion = true,
                 requiresBinding = true;
            string rawValue = null;
            object convertedValue = null;

            var descriptor = metadata.PropertyDescriptor;
            if (metadata.Position >= args.Args.Count)
            {
                //Невозможно привязать обязательный позиционный аргумент
                if (metadata.IsRequired)
                    throw new PositionalBindingException(metadata.Position);
                //Если он необязательный, то можно использовать значение по умолчанию
                if (descriptor.CanResetValue(target))
                    descriptor.ResetValue(target);
                requiresBinding = requiresConversion = false;
            }
            else
            {
                rawValue = args.Args[metadata.Position];
                boundFreeArgsPositions.Add(metadata.Position);
            }
            if (requiresConversion)
                convertedValue = ContextfulConvert(rawValue, target, metadata);
            if (requiresBinding)
                descriptor.SetValue(target, convertedValue);
        }

        //----------------------------------------------------------------------[]
        void BindUnboundOptions(OptionMetadata metadata, object target)
        {
            var unboundArgs = new List<string>();
            for (int i = 0; i < args.Args.Count; i++)
                if (!boundFreeArgsPositions.Contains(i))
                    unboundArgs.Add(args.Args[i]);

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

        static object ContextfulConvert(string text, object instance, OptionMetadata metadata)
        {
            var context = new BindingContext(instance, metadata);
            return metadata.PropertyDescriptor.Converter.ConvertFromString(context, CultureInfo.InvariantCulture, text);
        }

        public static TOptions BindTo<TOptions>(string[] args) where TOptions : new()
        {
            var parser = new ArgumentParser();
            return BindTo<TOptions>(parser.Parse(args));
        }

        //----------------------------------------------------------------------[]
        public static TOptions BindTo<TOptions>(CmdArgs args) where TOptions : new()
        {
            var binder = new OptionsBinder(args);
            return binder.Bind<TOptions>();
        }
    }
}