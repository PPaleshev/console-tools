using System.ComponentModel;

namespace ConsoleTools.Binding {
    /// <summary>
    /// Класс, представляющий собой метаданные, необходимые в процессе связывания значения.
    /// </summary>
    public class PropertyMetadata {
        /// <summary>
        /// Ключ опции.
        /// </summary>
        public PropertyKey Key { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Назначение свойства.
        /// </summary>
        public string Meaning { get; set; }

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
        /// Флаг, равный true, если аргумент содержит коллекцию элементов, false для скалярных значений.
        /// </summary>
        public bool IsCollection { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Описание опции.
        /// </summary>
        public string Description {
            get { return PropertyDescriptor.Description; }
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Тип свойства, определяющий правила её связывания.
        /// </summary>
        public Kind PropertyKind { get; set; }

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
        /// Разделитель элементов коллекции.
        /// </summary>
        public string CollectionItemSeparator { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр метаданных опции.
        /// </summary>
        public PropertyMetadata(PropertyDescriptor property)
        {
            PropertyKind = Kind.Unbound;
            PropertyDescriptor = property;
        }
    }
}