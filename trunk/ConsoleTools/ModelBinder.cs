using System;
using System.Collections;
using System.Collections.Generic;
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
        /// Региональные настройки, используемые при разборе.
        /// </summary>
        readonly CultureInfo culture;

        /// <summary>
        /// Создаёт новый экземпляр объекта.
        /// </summary>
        /// <param name="args">Хранилище переданных аргументов.</param>
        /// <param name="culture">Региональные настройки, используемые при разборе.</param>
        ModelBinder(CmdArgs args, CultureInfo culture)
        {
            this.args = args;
            this.culture = culture;
        }

        /// <summary>
        /// Выполняет привязку переданных аргументов <paramref name="args"/> к свойствам модели.
        /// </summary>
        /// <typeparam name="TModel">Тип модели.</typeparam>
        /// <param name="args">Переданные аргументы.</param>
        /// <param name="culture">Региональные настройки. Если на заданы, используется <see cref="CultureInfo.InvariantCulture"/>.</param>
        public static TModel BindTo<TModel>(string[] args, CultureInfo culture = null) where TModel : new()
        {
            var policyAttr = MetadataProvider.GetNamedArgumentsPolicy(typeof (TModel));
            var parser = policyAttr == null ? new ArgumentParser() : new ArgumentParser(new[] {policyAttr.Prefix}, new[] {policyAttr.Separator});
            return BindTo<TModel>(parser.Parse(args), culture);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Выполняет привязку аргументов, находящихся в хранилище <paramref name="args"/> к свойствам модели.
        /// </summary>
        /// <typeparam name="TModel">Тип модели.</typeparam>
        /// <param name="args">Хранилище аргументов.</param>
        /// <param name="culture">Региональные настройки. Если на заданы, используется <see cref="CultureInfo.InvariantCulture"/>.</param>
        public static TModel BindTo<TModel>(CmdArgs args, CultureInfo culture = null) where TModel : new()
        {
            try
            {
                var binder = new ModelBinder(args, culture ?? CultureInfo.CurrentCulture);
                return binder.Bind<TModel>();
            }
            catch (BindingException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BindingException("Error while model binding occured", e);
            }
        }

        /// <summary>
        /// Выполняет привязку аргументов к свойствам модели.
        /// </summary>
        /// <typeparam name="TModel">Тип модели, содержащей привязываемые свойства.</typeparam>
        /// <returns>Возвращает объект с установленными значениями свойств.</returns>
        TModel Bind<TModel>() where TModel : new()
        {
            var propertyMetadata = MetadataProvider.ReadPropertyMetadata(typeof (TModel));
            var model = new TModel();
            var propertyGroups = propertyMetadata.GroupBy(metadata => metadata.PropertyKind).ToDictionary(g => g.Key, g => g.ToList());
            List<PropertyMetadata> properties;
            if (propertyGroups.TryGetValue(Kind.Named, out properties))
                BindNamedOptions(properties, model);
            if (propertyGroups.TryGetValue(Kind.Positional, out properties))
                BindPositionalOptions(properties, model);
            if (propertyGroups.TryGetValue(Kind.Unbound, out properties))
                CollectUnboundOptions(properties, model);
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
        void CollectUnboundOptions(IList<PropertyMetadata> properties, object target)
        {
            if (properties.Count > 1)
                throw new InvalidOperationException("Only one property can be marked with UnboundAttribute");
            if (properties.Count == 0)
                return;
            var unboundOptionsTarget = properties[0];
            if (unboundOptionsTarget != null)
                BindUnboundOptions(unboundOptionsTarget, target);
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
            var key = metadata.Specification.Key;
            if (metadata.Specification.IsSwitch)
            {
                requiresConversion = false;
                convertedValue = args.Contains(key.Name) || (key.HasAlias && args.Contains(key.Alias));
            }
            else if (args.TryGetNamedValue(key.Name, out temp))
                rawValue = temp;
            else if (key.HasAlias && args.TryGetNamedValue(key.Alias, out temp))
                rawValue = temp;
            else if (metadata.IsRequired)
                throw new MissingRequiredOptionException(metadata);
            else
            {
                requiresConversion = false;
                requiresBinding = !ReferenceEquals(null, metadata.DefaultValue);
                if (requiresBinding)
                    convertedValue = metadata.DefaultValue;
            }
            if (requiresConversion)
                convertedValue = ConvertValue(rawValue, metadata);
            if (requiresBinding)
                SetValue(metadata, target, convertedValue);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Привязывает значение позиционного аргумента к свойству целевого объекта.
        /// </summary>
        /// <param name="metadata">Метаданные свойства.</param>
        /// <param name="target">Целевой объект.</param>
        void BindPositionalOption(PropertyMetadata metadata, object target)
        {
            var position = metadata.Specification.Position;
            object value;
            if (position >= args.UnboundValues.Count)
            {
                if (metadata.IsRequired)
                    throw new MissingRequiredOptionException(metadata);
                value = metadata.DefaultValue;
            }
            else
            {
                var rawValue = args.UnboundValues[position];
                boundFreeArgsPositions.Add(position);
                value = ConvertValue(rawValue, metadata);
            }
            SetValue(metadata, target, value);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Собирает все непривязанные аргументы в специальное свойство.
        /// Для несвязанных аргументов преобразование элементов не выполняется.
        /// </summary>
        /// <param name="metadata">Метаданные свойства, в которое должны быть помещены все несвязанные аргументы.</param>
        /// <param name="target">Целевой объект.</param>
        void BindUnboundOptions(PropertyMetadata metadata, object target)
        {
            var unboundArgs = args.UnboundValues.Where((t, i) => !boundFreeArgsPositions.Contains(i)).ToList();
            if (metadata.Property.PropertyType.IsArray)
                SetValue(metadata, target, unboundArgs.ToArray());
            else
            {
                var list = Activator.CreateInstance(metadata.Property.PropertyType) as IList;
                foreach (string arg in unboundArgs)
                    list.Add(arg);
                SetValue(metadata, target, list);
            }
        }

        /// <summary>
        /// Устанавливает значение свойства.
        /// </summary>
        static void SetValue(PropertyMetadata meta, object instance,  object value)
        {
            meta.Property.SetValue(instance, value, null);
        }

        /// <summary>
        /// Выполняет преобразование значения свойства из строки в реальный тип свойства, используя конвертер.
        /// </summary>
        /// <param name="text">Строковое значение.</param>
        /// <param name="metadata">Метаданные свойства.</param>
        /// <returns>Возвращает преобразованное значение свойства.</returns>
        object ConvertValue(string text, PropertyMetadata metadata)
        {
            return metadata.Converter.ConvertFromString(text, culture, metadata.Property.PropertyType);
        }
    }
}