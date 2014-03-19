namespace ConsoleTools
{
    /// <summary>
    /// Интерфейс, предоставляющий доступ к данным, необходимым для формирования лого.
    /// </summary>
    public interface IApplicationDataProvider
    {
        /// <summary>
        /// Возвращает название исполняемого файла.
        /// </summary>
        string ApplicationExeName { get; }

        /// <summary>
        /// Описание используемого приложения.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Версия приложения.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Полное текстовое описание приложения.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Информация об авторах приложения и объекте авторского права.
        /// </summary>
        string Copyright { get; }
    }
}