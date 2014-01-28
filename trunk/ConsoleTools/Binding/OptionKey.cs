using System;


namespace ConsoleTools.Binding {
    /// <summary>
    /// Ключ опции. Включает в себя полное название опции и его псевдоним.
    /// </summary>
    public struct OptionKey
    {
        /// <summary>
        /// Название опции. Обязательно для заполнения.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Псевдоним опции. Необязателен для указания.
        /// </summary>
        public readonly string Alias;

        /// <summary>
        /// Флаг, равный true, если текущий ключ содержит краткое название свойства, иначе false.
        /// </summary>
        public bool HasAlias
        {
            get { return !string.IsNullOrEmpty(Alias); }
        }

        /// <summary>
        /// Создаёт новый экземпляр структуры с указанием названия опции и её псевдонима.
        /// </summary>
        /// <param name="name">Название опции.</param>
        /// <param name="alias">Псевдоним опции (краткое наименование).</param>
        public OptionKey(string name, string alias)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            Name = name;
            Alias = alias;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Создаёт новый экземпляр структуры без краткого названия.
        /// </summary>
        /// <param name="name">Наименование опции.</param>
        public OptionKey(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            Name = name;
            Alias = string.Empty;
        }

        /// <summary>
        /// Оператор неявного преобразования строкового значения в формате {name};{alias} в ключ опции.
        /// </summary>
        /// <param name="value">Строка в формате {name}[;{alias}], где name - обязательное имя свойства, а alias - опциональный псевдоним.</param>
        /// <returns>Возвращает новый экземпляр ключа опции.</returns>
        public static implicit operator OptionKey(string value) {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            var values = value.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length < 1 || values.Length > 2)
                throw new FormatException("Invalid value format. Expected '<name>[;alias]' format");
            return values.Length == 2 ? new OptionKey(values[0], values[1]) : new OptionKey(values[0]);
        }

        //----------------------------------------------------------------------[]
        public override string ToString() {
            var result = Name;
            if (HasAlias)
                result += ";" + Alias;
            return result;
        }
    }
}