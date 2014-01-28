namespace ConsoleTools.Binding {
    /// <summary>
    /// Перечисление, определяющее тип аргумента.
    /// </summary>
    public enum OptionType {
        /// <summary>
        /// Свободный аргумент.
        /// </summary>
        Unbound,

        /// <summary>
        /// Именованный аргумент.
        /// </summary>
        Named,

        /// <summary>
        /// Позиционный аргумент.
        /// </summary>
        Positional
    }
}