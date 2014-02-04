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
    /// Класс для осуществления привязки значений переданных аргументов значениям свойств указанной модели.
    /// </summary>
    public class OptionsBinder
    {
        /// <summary>
        /// Объект-хранилище для переданных аргументов.
        /// </summary>
        readonly CmdArgs args;
        
        /// <summary>
        /// Список метаданных всех свойств модели.
        /// </summary>
        readonly IList<OptionMetadata> optionsMetadata = new List<OptionMetadata>();

        /// <summary>
        /// Список номеров позиций, для которых удалось осуществить привязку позиционных аргументов.
        /// </summary>
        readonly HashSet<int> boundFreeArgsPositions = new HashSet<int>();

        /// <summary>
        /// Создаёт новый экземпляр объекта.
        /// </summary>
        /// <param name="args">Хранилище переданных аргументов.</param>
        OptionsBinder(CmdArgs args)
        {
            this.args = args;
        }

        /// <summary>
        /// Выполняет привязку переданных аргументов <paramref name="args"/> к свойствам модели.
        /// </summary>
        /// <typeparam name="TOptions">Тип модели.</typeparam>
        /// <param name="args">Переданные аргументы.</param>
        public static TOptions BindTo<TOptions>(string[] args) where TOptions : new()
        {
            var parser = new ArgumentParser();
            return BindTo<TOptions>(parser.Parse(args));
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Выполняет привязку аргументов, находящихся в хранилище <paramref name="args"/> к свойствам модели.
        /// </summary>
        /// <typeparam name="TOptions">Тип модели.</typeparam>
        /// <param name="args">Хранилище аргументов.</param>
        public static TOptions BindTo<TOptions>(CmdArgs args) where TOptions : new()
        {
            var binder = new OptionsBinder(args);
            return binder.Bind<TOptions>();
        }

        /// <summary>
        /// Выполняет привязку аргументов к свойствам модели.
        /// </summary>
        /// <typeparam name="TOptions">Тип модели, содержащей привязываемые свойства.</typeparam>
        /// <returns>Возвращает объект с установленными значениями свойств.</returns>
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
        /// Извлекает метаданные из свойств модели.
        /// </summary>
        /// <param name="optionsType">Тип модели.</param>
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
        /// Выполняет привязку именованных аргументов к свойствам объекта.
        /// </summary>
        /// <param name="target">Целевой объект.</param>
        void BindNamedOptions(object target)
        {
            foreach (var metadata in optionsMetadata.Where(metadata => metadata.OptionType == OptionType.Named))
                BindNamedOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Выполняет привязку позиционных аргументов к свойствам объекта.
        /// </summary>
        /// <param name="target">Целевой объект.</param>
        void BindPositionalOptions(object target)
        {
            foreach (var metadata in optionsMetadata.Where(metadata => metadata.OptionType == OptionType.Positional))
                BindPositionalOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Собирает все непривязанные аргументы.
        /// </summary>
        /// <param name="target">Целевой объект.</param>
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
        /// Привязывает значение именованного аргумента к свойству целевого объекта.
        /// </summary>
        /// <param name="metadata">Метаданные свойства.</param>
        /// <param name="target">Целевой объект.</param>
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
        /// Привязывает значение позиционного аргумента к свойству целевого объекта.
        /// </summary>
        /// <param name="metadata">Метаданные свойства.</param>
        /// <param name="target">Целевой объект.</param>
        void BindPositionalOption(OptionMetadata metadata, object target)
        {
            bool requiresConversion = true,
                 requiresBinding = true;
            string rawValue = null;
            object convertedValue = null;

            var descriptor = metadata.PropertyDescriptor;
            if (metadata.Position >= args.UnboundValues.Count)
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
        /// Собирает все непривязанные аргументы в специальное свойство, если такое имеется.
        /// </summary>
        /// <param name="metadata">Метаданные свойства, в которое должны быть помещены все несвязанные аргументы.</param>
        /// <param name="target">Целевой объект.</param>
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