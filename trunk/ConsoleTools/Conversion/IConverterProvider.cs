using System;

namespace ConsoleTools.Conversion
{
    /// <summary>
    /// Интерфейс объекта, способного вернуть конвертер по указанному типу.
    /// </summary>
    public interface IConverterProvider
    {
        /// <summary>
        /// Возвращает конвертер для указанного типа, если этот тип поддерживается поставщиком.
        /// </summary>
        /// <param name="type">Тип, конвертер для которого необходимо получить.</param>
        IConverter GetConverter(Type type);
    }
}