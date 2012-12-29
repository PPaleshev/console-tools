using System;
using ConsoleTools.Utils;


namespace ConsoleTools.Binding {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class OptionBindingAttribute : Attribute {
        #region Data

        private readonly bool _isRequired;
        private object _defaultValue;

        #endregion

        #region Properties

        //----------------------------------------------------------------------[]
        /// <summary>
        /// Обязательность аргумента
        /// </summary>
        public bool IsRequired {
            get { return _isRequired; }
        }

        #endregion

        #region Construction

        protected OptionBindingAttribute(bool isRequired) {
            _isRequired = isRequired;
        }

        #endregion

        #region Methods

        public abstract void FillMetadata(OptionMetadata metadata);

        #endregion
    }
}