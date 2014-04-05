using ConsoleTools.Binding;

namespace ConsoleTools.Exceptions {
    /// <summary>
    /// Исключение, возникающее при отсутствии значения обязательного параметра.
    /// </summary>
    public class MissingRequiredOptionException : BindingException {
        /// <summary>
        /// Метаданные свойства, при связывании которого возникло исключение.
        /// </summary>
        public PropertyMetadata Metadata { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр исключения.
        /// </summary>
        /// <param name="metadata">Метаданные свойства, при связывании которого возникла ошибка.</param>
        public MissingRequiredOptionException(PropertyMetadata metadata) : base("Missing required option")
        {
            Metadata = metadata;
        }
    }
}