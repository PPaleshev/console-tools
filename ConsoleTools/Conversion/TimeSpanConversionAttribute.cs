using System;
using ConsoleTools.Binding;

namespace ConsoleTools.Conversion
{
    /// <summary>
    /// Атрибут для управления разбором аргументов типа <see cref="TimeSpan"/> .
    /// </summary>
    public class TimeSpanConversionAttribute : SpecificationAttribute, IConverterProvider
    {
        /// <summary>
        /// Формат разбора.
        /// </summary>
        readonly string format;

        /// <summary>
        /// Создаёт новый экземпляр атрибута с указанием формата представления <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="format">Формат представления.</param>
        public TimeSpanConversionAttribute(string format)
        {
            this.format = format;
        }

        public override void UpdateSpecification(PropertySpecification spec)
        {
            spec.Format = format;
        }

        public IConverter GetConverter(Type type)
        {
            if (!TimeSpanConverter.CanConvertTo(type))
                throw new InvalidOperationException(string.Format("{0} can convert strings only to TimeSpan type", typeof (TimeSpanConverter).Name));
            return new TimeSpanConverter(format);
        }
    }
}