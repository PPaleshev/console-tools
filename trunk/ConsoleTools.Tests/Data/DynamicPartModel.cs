using System;
using ConsoleTools.Binding;

namespace ConsoleTools.Tests.Data
{
    /// <summary>
    /// Модель, содержащая динамические части.
    /// </summary>
    public class DynamicPartModel
    {
        IParams p;

        [Named("command;cmd")]
        public string Command { get; set; }

        /// <summary>
        /// Динамические параметры.
        /// </summary>
        [Part(true)]
        public IParams Params
        {
            get
            {
                if (p == null)
                {
                    switch (Command)
                    {
                        case "create":
                            p = new CreateParams();
                            break;
                        case "remove":
                            p = new RemoveParams();
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                return p;
            }
        }
    }

    /// <summary>
    /// Интерфейс параметров.
    /// </summary>
    public interface IParams
    {
    }

    /// <summary>
    /// Параметры для команды 1.
    /// </summary>
    public class CreateParams : IParams
    {
        [Named("name", IsRequired = true)]
        public string Name { get; set; }

        [Named("value", IsRequired = true)]
        public string Value { get; set; }
    }

    /// <summary>
    /// Параметры для команды 2.
    /// </summary>
    public class RemoveParams : IParams
    {
        [Named("id", IsRequired = true)]
        public string Id { get; set; }
    }
}