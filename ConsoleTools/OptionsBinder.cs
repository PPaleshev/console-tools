using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ConsoleTools.Binding;
using ConsoleTools.Exceptions;
using ConsoleTools.Utils;


namespace ConsoleTools {
    public class OptionsBinder {
        #region Data

        private readonly CmdArgs _args;
        private readonly IList<OptionMetadata> _optionsMetadata = new List<OptionMetadata>();
        private readonly IList<OptionMetadata> _missingRequiredOptions = new List<OptionMetadata>();
        private readonly List<int> _boundFreeArgsPositions = new List<int>();

        #endregion

        #region Construction

        private OptionsBinder(CmdArgs args) {
            _args = args;
        }

        #endregion

        #region Methods

        private TOptions Bind<TOptions>() where TOptions : new() {
            ExtractDeclaredOptionDefinitions(typeof (TOptions));

            TOptions optionsObject = new TOptions();

            BindNamedOptions(optionsObject);
            BindPositionalOptions(optionsObject);
            BindUnboundOptions(optionsObject);

            return optionsObject;
        }

        #endregion

        #region Routines

        private void ExtractDeclaredOptionDefinitions(Type optionsType) {
            PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(optionsType);
            foreach (PropertyDescriptor property in descriptors) {
                OptionBindingAttribute attr =
                    (OptionBindingAttribute) property.Attributes[typeof (OptionBindingAttribute)];

                if (attr == null) {
                    continue;
                }

                OptionMetadata metadata = new OptionMetadata(property);
                metadata.IsRequired = attr.IsRequired;
                metadata.IsFlag = property.Attributes[typeof (FlagAttribute)] != null;
                attr.FillMetadata(metadata);

                _optionsMetadata.Add(metadata);
            }
        }

        //----------------------------------------------------------------------[]
        private void BindNamedOptions(object optionsObject) {
            foreach (OptionMetadata metadata in Filters.GetNamedArguments(_optionsMetadata)) {
                BindNamedOption(metadata, optionsObject);
            }
        }

        //----------------------------------------------------------------------[]
        private void BindPositionalOptions(object optionsObject) {
            foreach (OptionMetadata metadata in Filters.GetPositionalArguments(_optionsMetadata)) {
                BindPositionalOption(metadata, optionsObject);
            }
        }

        //----------------------------------------------------------------------[]
        private void BindUnboundOptions(object optionsObject) {
            try {
                OptionMetadata unboundOptionsTarget = Filters.GetSingleUnboundOptionsMetadataOrThrow(_optionsMetadata);
                if (unboundOptionsTarget != null) {
                    BindUnboundOptions(unboundOptionsTarget, optionsObject);
                }
            } catch (ArgumentException) {
                throw new BindingException("Only one property can contain unbound options");
            }
        }

        //----------------------------------------------------------------------[]
        private void BindNamedOption(OptionMetadata metadata, object target) {
            string rawValue = null;
            object convertedValue = null;
            bool needConversion = true,
                 needBinding = true;

            if (metadata.IsFlag) {
                needConversion = false;
                convertedValue = _args.Contains(metadata.Key.Name)
                                 || (metadata.Key.HasAlias && _args.Contains(metadata.Key.Alias));
            } else if (_args.Contains(metadata.Key.Name)) {
                rawValue = _args[metadata.Key.Name];
            } else if (metadata.Key.HasAlias && _args.Contains(metadata.Key.Alias)) {
                rawValue = _args[metadata.Key.Alias];
            } else if (metadata.IsRequired) {
                throw new MissingRequiredOptionException(metadata);
            } else {
                if (metadata.PropertyDescriptor.CanResetValue(target)) {
                    metadata.PropertyDescriptor.ResetValue(target);
                }
                needConversion = false;
                needBinding = false;
            }

            if (needConversion) {
                convertedValue = metadata.PropertyDescriptor
                    .Converter
                    .ConvertFromString(rawValue);
            }

            if (needBinding) {
                metadata.PropertyDescriptor.SetValue(target, convertedValue);
            }
        }

        //----------------------------------------------------------------------[]
        private void BindPositionalOption(OptionMetadata metadata, object target) {
            bool needConversion = true,
                 needBinding = true;
            string rawValue = null;
            object convertedValue = null;

            PropertyDescriptor descriptor = metadata.PropertyDescriptor;

            if (metadata.Position >= _args.Args.Count) {
                //Невозможно привязать обязательный позиционный аргумент
                if (metadata.IsRequired) {
                    throw new PositionalBindingException(metadata.Position);
                }

                //Если он необязательный, то можно использовать значение по умолчанию
                if (descriptor.CanResetValue(target)) {
                    descriptor.ResetValue(target);
                }
                needBinding =
                    needConversion = false;
            } else {
                rawValue = _args.Args[metadata.Position];
                _boundFreeArgsPositions.Add(metadata.Position);
            }

            if (needConversion) {
                convertedValue = descriptor
                    .Converter.ConvertFromString(rawValue);
            }

            if (needBinding) {
                descriptor.SetValue(target, convertedValue);
            }
        }

        //----------------------------------------------------------------------[]
        private void BindUnboundOptions(OptionMetadata metadata, object target) {
            List<string> unboundArgs = new List<string>();

            for (int i = 0; i < _args.Args.Count; i++) {
                if (!_boundFreeArgsPositions.Contains(i)) {
                    unboundArgs.Add(_args.Args[i]);
                }
            }

            PropertyDescriptor descriptor = metadata.PropertyDescriptor;
            if (descriptor.PropertyType.IsArray) {
                descriptor.SetValue(target, unboundArgs.ToArray());
            } else {
                IList list = Activator.CreateInstance(descriptor.PropertyType) as IList;
                foreach (string arg in unboundArgs) {
                    list.Add(arg);
                }
                descriptor.SetValue(target, list);
            }
        }

        #endregion

        #region Static

        public static TOptions BindTo<TOptions>(string[] args) where TOptions : new() {
            ArgumentParser parser = new ArgumentParser();
            return BindTo<TOptions>(parser.Parse(args));
        }

        //----------------------------------------------------------------------[]
        public static TOptions BindTo<TOptions>(CmdArgs args) where TOptions : new() {
            OptionsBinder binder = new OptionsBinder(args);
            return binder.Bind<TOptions>();
        }

        #endregion
    }
}