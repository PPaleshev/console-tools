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
    /// Класс для привязки значений переданных аргументов значениям свойств указанной модели.
    /// </summary>
    public class ModelBinder
    {
        /// <summary>
        /// Объект-хранилище для переданных аргументов.
        /// </summary>
        readonly CmdArgs args;
        
        /// <summary>
        /// Список номеров позиций, для которых удалось осуществить привязку позиционных аргументов.
        /// </summary>
        readonly HashSet<int> boundFreeArgsPositions = new HashSet<int>();

        /// <summary>
        /// Создаёт новый экземпляр объекта.
        /// </summary>
        /// <param name="args">Хранилище переданных аргументов.</param>
        ModelBinder(CmdArgs args)
        {
            this.args = args;
        }

        /// <summary>
        /// Выполняет привязку переданных аргументов <paramref name="args"/> к свойствам модели.
        /// </summary>
        /// <typeparam name="TModel">Тип модели.</typeparam>
        /// <param name="args">Переданные аргументы.</param>
        public static TModel BindTo<TModel>(string[] args) where TModel : new()
        {
            var policyAttr = (NamedArgumentsPolicyAttribute)Attribute.GetCustomAttribute(typeof(TModel), typeof(NamedArgumentsPolicyAttribute));
            var parser = policyAttr == null ? new ArgumentParser() : new ArgumentParser(new[] {policyAttr.Prefix}, new[] {policyAttr.Separator});
            return BindTo<TModel>(parser.Parse(args));
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Выполняет привязку аргументов, находящихся в хранилище <paramref name="args"/> к свойствам модели.
        /// </summary>
        /// <typeparam name="TModel">Тип модели.</typeparam>
        /// <param name="args">Хранилище аргументов.</param>
        public static TModel BindTo<TModel>(CmdArgs args) where TModel : new()
        {
            var binder = new ModelBinder(args);
            return binder.Bind<TModel>();
        }

        /// <summary>
        /// Выполняет привязку аргументов к свойствам модели.
        /// </summary>
        /// <typeparam name="TModel">Тип модели, содержащей привязываемые свойства.</typeparam>
        /// <returns>Возвращает объект с установленными значениями свойств.</returns>
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
        /// Выполняет привязку именованных аргументов к свойствам объекта.
        /// </summary>
        /// <param name="properties">Перечисление метаданных именованных свойств модели.</param>
        /// <param name="target">Целевой объект.</param>
        void BindNamedOptions(IEnumerable<PropertyMetadata> properties, object target)
        {
            foreach (var metadata in properties.Where(metadata => metadata.PropertyKind == Kind.Named))
                BindNamedOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Выполняет привязку позиционных аргументов к свойствам объекта.
        /// </summary>
        /// <param name="properties">Перечисление метаданных позиционных свойств модели.</param>
        /// <param name="target">Целевой объект.</param>
        void BindPositionalOptions(IEnumerable<PropertyMetadata> properties, object target)
        {
            foreach (var metadata in properties)
                BindPositionalOption(metadata, target);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Собирает все непривязанные аргументы.
        /// </summary>
        /// <param name="properties">Перечисление метаданных несвязанных свойств модели.</param>
        /// <param name="target">Целевой объект.</param>
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
        /// Привязывает значение именованного аргумента к свойству целевого объекта.
        /// </summary>
        /// <param name="metadata">Метаданные свойства.</param>
        /// <param name="target">Целевой объект.</param>
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
        /// Привязывает значение позиционного аргумента к свойству целевого объекта.
        /// </summary>
        /// <param name="metadata">Метаданные свойства.</param>
        /// <param name="target">Целевой объект.</param>
        void BindPositionalOption(PropertyMetadata metadata, object target)
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
        /// Выполняет преобразование значения свойства из строки в реальный тип свойства, используя конвертер из <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="text">Строковое значение.</param>
        /// <param name="instance">Экземпляр модели.</param>
        /// <param name="metadata">Метаданные свойства.</param>
        /// <returns>Возвращает преобразованное значение свойства.</returns>
        static object ContextfulConvert(string text, object instance, PropertyMetadata metadata)
        {
            var context = new BindingContext(instance, metadata);
            return metadata.PropertyDescriptor.Converter.ConvertFromString(context, CultureInfo.InvariantCulture, text);
        }
    }
}