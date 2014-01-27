using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleTools {

    /// <summary>
    /// Промежуточное представление разобранных аргументов командной строки.
    /// </summary>
    public class CmdArgs
    {
        /// <summary>
        /// Словарь именованных аргументов.
        /// Представлен отображением названия аргумента в его значение.
        /// </summary>
        readonly IDictionary<string, string> namedArgs;

        /// <summary>
        /// Список непривязанных аргументов в порядке их появления в списке аргументов.
        /// </summary>
        readonly IList<string> unboundArgs = new List<string>();

        /// <summary>
        /// Коллекция неименованных аргументов в порядке их появления в списке аргументов.
        /// </summary>
        public ReadOnlyCollection<string> Args {
            get { return new ReadOnlyCollection<string>(unboundArgs); }
        }

        /// <summary>
        /// Создаёт новый экземпляр класса для представления разобранных аргументов.
        /// </summary>
        /// <param name="namedArgs">Отображение из имени параметра в его значение.</param>
        /// <param name="unboundArgs">Список непривязанных аргументов.</param>
        public CmdArgs(IDictionary<string, string> namedArgs, IList<string> unboundArgs)
        {
            this.namedArgs = namedArgs ?? new Dictionary<string, string>();
            this.unboundArgs = unboundArgs ?? new ReadOnlyCollection<string>(new string[0]);
        }

        /// <summary>
        /// Создаёт пустой экземпляр для представления разобранных аргументов.
        /// </summary>
        public CmdArgs() {
        }

        /// <summary>
        /// Добавляет значение именованного аргумента.
        /// </summary>
        /// <param name="name">Название аргумента.</param>
        /// <param name="value">Значение аргумента.</param>
        public void AddNamedArg(string name, string value) {
            namedArgs[name] = value;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Добавляет несвязанный аргумент.
        /// </summary>
        /// <param name="arg">Значение несвязанного аргумента.</param>
        public void AddUnboundArg(string arg) {
            unboundArgs.Add(arg);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Очищает списки аргументов.
        /// </summary>
        public void Clear() {
            namedArgs.Clear();
            unboundArgs.Clear();
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Определяет, есть ли аргумент с указанным именем в списке именованных аргументов.
        /// </summary>
        /// <param name="name">Имя аргумента.</param>
        /// <returns>Возвращает true, если аргумент есть среди именованных аргументов, иначе false.</returns>
        public bool Contains(string name) {
            return namedArgs.ContainsKey(name);
        }

        /// <summary>
        /// Пытается вернуть значение по его имени.
        /// </summary>
        /// <param name="name">Имя параметра.</param>
        /// <param name="value">Значение параметра.</param>
        /// <returns>Возвращает true, если значение параметра с указанным именем существует, иначе false.</returns>
        public bool TryGetNamedValue(string name, out string value)
        {
            return namedArgs.TryGetValue(name, out value);
        }
    }
}