using System;
using System.Collections;
using System.Collections.Generic;


namespace ConsoleTools.Conversion {
    public class ValueListConverterAttribute : OptionConverterAttribute {
        #region Data

        private string _separator = ";";
        private bool _ignoreEmptyItems;
        private readonly Type _itemType = typeof (string);

        #endregion

        #region Properties

        public string Separator {
            get { return _separator; }
            set { _separator = value; }
        }

        //----------------------------------------------------------------------[]
        public bool IgnoreEmptyItems {
            get { return _ignoreEmptyItems; }
            set { _ignoreEmptyItems = value; }
        }

        //----------------------------------------------------------------------[]
        public Type ItemType {
            get { return _itemType; }
        }

        #endregion

        #region Construction

        public ValueListConverterAttribute(Type itemType) {
            _itemType = itemType;
        }

        //----------------------------------------------------------------------[]
        public ValueListConverterAttribute() {
        }

        #endregion

        #region Overrides

        public override object Convert(string input, Type targetType) {
            string[] items = input.Split(new string[] {_separator},
                                         _ignoreEmptyItems
                                             ? StringSplitOptions.RemoveEmptyEntries
                                             : StringSplitOptions.None);
            ArrayList convertedItems = ConvertSourceItems(items);

            return targetType.IsArray
                       ? convertedItems.ToArray(ItemType)
                       : FormatAsList(targetType, convertedItems);
        }

        #endregion

        #region Routines

        private ArrayList ConvertSourceItems(IEnumerable<string> items) {
            ArrayList list = new ArrayList();
            foreach (string item in items) {
                list.Add(DefaultOptionConverterAttribute.Instance.Convert(item, _itemType));
            }
            return list;
        }

        //----------------------------------------------------------------------[]
        private static IList FormatAsList(Type collectionType, ArrayList items) {
            IList list = (IList) Activator.CreateInstance(collectionType);

            foreach (object item in items) {
                list.Add(item);
            }

            return list;
        }
        #endregion
    }
}