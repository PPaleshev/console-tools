using System;
using System.Globalization;

namespace ConsoleTools.Conversion
{
    /// <summary>
    /// Интерфейс, обеспечивающий преобразование из строки в объекты .NET.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Преобразует указанное строковое значение в экземпляр указанного типа.
        /// </summary>
        /// <param name="value">Строковое значение.</param>
        /// <param name="culture">Культура, используемая при преобразовании.</param>
        /// <param name="targetType">Тип значения, в который нужно преобразовать строку.</param>
        /// <returns>Возвращает объект, полученный после преобразования из строки.</returns>
        object ConvertFromString(string value, CultureInfo culture, Type targetType);
    }
}
