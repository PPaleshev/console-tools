using System.Reflection;

namespace ConsoleTools.Utils
{
    /// <summary>
    /// Реализация провайдера информации о заявлении, получающая её из метаданных сборки, содержащей точку входа. 
    /// </summary>
    class EntryAssemblyDataProvider : IApplicationDataProvider
    {
        /// <summary>
        /// Создаёт новый экземпляр объекта с информацией о приложении.
        /// </summary>
        public EntryAssemblyDataProvider()
        {
            var assembly = Assembly.GetEntryAssembly();
        }

        public string ApplicationExeName { get; private set; }
        public string Title { get; private set; }
        public string Version { get; private set; }
        public string Description { get; private set; }
        public string Copyright { get; private set; }
    }
}