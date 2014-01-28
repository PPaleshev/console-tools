using System.ComponentModel;

namespace ConsoleTools.Binding {
    /// <summary>
    /// Класс, представляющий собой метаданные, необходимые в процессе связывания значения.
    /// </summary>
    public class OptionMetadata {
        /// <summary>
        /// Ключ опции.
        /// </summary>
        public OptionKey Key { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Флаг, равный true, если значение опции обязательно для указания, иначе false.
        /// </summary>
        public bool IsRequired { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Флаг, равный true, если значение опции является "переключателем" или флагом.
        /// </summary>
        public bool IsSwitch { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Описание опции.
        /// </summary>
        public string Description {
            get { return PropertyDescriptor.Description; }
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Тип опции, определяющий правила её связывания.
        /// </summary>
        public OptionType OptionType { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Порядковый номер опции, используемый при связывании позиционных параметров.
        /// </summary>
        public int Position { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Дескриптор свойства.
        /// </summary>
        public PropertyDescriptor PropertyDescriptor { get; private set; }

        /// <summary>
        /// Создаёт новый экземпляр метаданных опции.
        /// </summary>
        public OptionMetadata(PropertyDescriptor property)
        {
            OptionType = OptionType.Unbound;
            PropertyDescriptor = property;
        }
    }
}