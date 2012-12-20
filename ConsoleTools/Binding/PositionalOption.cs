using System;


namespace ConsoleTools.Binding {
    public class PositionalOption : OptionBindingAttribute {
        #region Data

        private readonly int _position = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Номер привязываемого атрибута
        /// </summary>
        public int Position {
            get { return _position; }
        }

        #endregion

        #region Construction

        public PositionalOption(int position, bool isRequired) : base(isRequired) {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");

            _position = position;
        }

        //----------------------------------------------------------------------[]
        public PositionalOption(int position)
            : this(position, false) {
        }

        #endregion

        #region Overrides

        public override void FillMetadata(OptionMetadata metadata) {
            metadata.ArgumentType = ArgumentType.Positional;
            metadata.Position = _position;
        }

        #endregion

    }
}