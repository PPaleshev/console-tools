using System;
using ConsoleTools.Binding;

namespace ConsoleTools.Conversion
{
    /// <summary>
    /// Атрибут для управления разбором аргументов типа <see cref="DateTime"/> .
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class DateTimeConversionAttribute : SpecificationAttribute, IConverterProvider
    {
        /// <summary>
        /// Формат разбора.
        /// </summary>
        readonly string format;

        /// <summary>
        /// Создаёт новый экземпляр атрибута с указанием формата разбора <see cref="DateTime"/>.
        /// </summary>
        /// <param name="format">Формат представления.</param>
        public DateTimeConversionAttribute(string format)
        {
            this.format = format;
        }

        public override void UpdateSpecification(PropertySpecification spec)
        {
            spec.Format = format;
        }

        public IConverter GetConverter(Type type)
        {
            if (!DateTimeConverter.CanConvertTo(type))
                throw new InvalidOperationException(string.Format("{0} can convert strings only to DateTime type", typeof (DateTimeConverter).Name));
            return new DateTimeConverter(format);
        }
    }
}