// Павел "Nogard" Палешев [dragon.metalheart@gmail.com]

using System;

namespace ConsoleTools {
    /// <summary>
    /// Класс, выполняющий разбор списка аргументов, переданных при вызове приложения.
    /// </summary>
    public class ArgumentParser {
        /// <summary>
        /// Массив префиксов по умолчанию.
        /// </summary>
        static readonly string[] DefaultPrefixes = new[] {"/", "-", "--"};

        /// <summary>
        /// Массив разделителей имени и значения в именованных параметрах по умолчанию.
        /// </summary>
        static readonly char[] DefaultSeparators = new[] {':', '='};

        /// <summary>
        /// Массив разделителей имени и значения при разборе именованных параметров.
        /// </summary>
        readonly char[] separators;

        /// <summary>
        /// Массив префиксов именованных параметров.
        /// </summary>
        readonly string[] prefixes;

        /// <summary>
        /// Создаёт новый экземпляр парсера с префиксами и разделителями по умолчанию.
        /// </summary>
        public ArgumentParser()
        {
            separators = DefaultSeparators;
            prefixes = DefaultPrefixes;
        }

        /// <summary>
        /// Создаёт новый экземпляр парсера.
        /// </summary>
        /// <param name="prefixes">Массив префиксов для разбора именованных параметров.</param>
        /// <param name="separators">Массив разделителей.</param>
        public ArgumentParser(string[] prefixes, char[] separators)
        {
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            if (separators == null || separators.Length == 0)
                throw new ArgumentException("separators");
            this.prefixes = prefixes;
            this.separators = separators;
        }

        /// <summary>
        /// Выполняет разбор переданных аргументов.
        /// </summary>
        /// <param name="args">Аргументы для разбора.</param>
        /// <returns></returns>
        public CmdArgs Parse(params string[] args) {
            var result = new CmdArgs();

            foreach (var arg in args) {
                if (string.IsNullOrEmpty(arg))
                    continue;

                string argPrefix;
                if (!CheckIsNamedArgument(arg, out argPrefix)) {
                    result.AddUnboundArg(arg);
                    continue;
                }

                var separatorIndex = arg.IndexOfAny(separators, argPrefix.Length);
                var option = arg.Substring(argPrefix.Length, separatorIndex == -1 ? arg.Length - argPrefix.Length : separatorIndex - argPrefix.Length);
                //Если в аргументе не найдено разделителей, то это флаг
                var optionValue = separatorIndex == -1 ? string.Empty : arg.Substring(separatorIndex + 1, arg.Length - separatorIndex - 1);
                result.AddNamedArg(option, optionValue);
            }

            return result;
        }

        /// <summary>
        /// Проверяет, является ли переданный аргумент именованным значением.
        /// </summary>
        /// <param name="value">Проверяемый аргумент.</param>
        /// <param name="prefix">Возвращаемое значение, представляющее собой префикс, которым отмечаются именованные аргументы.</param>
        /// <returns>Возвращает true, если переданный аргумент является именованным значением, иначе false.</returns>
        bool CheckIsNamedArgument(string value, out string prefix) {
            foreach (string argprefix in prefixes) {
                if (value.StartsWith(argprefix)) {
                    prefix = argprefix;
                    return true;
                }
            }
            prefix = string.Empty;
            return false;
        }
    }
}