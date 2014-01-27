using System;
using System.Collections.Generic;
using ConsoleTools.Binding;


namespace ConsoleTools.Utils {
    internal static class Filters {
        #region Methods

        public static IList<OptionMetadata> GetNamedArguments(IList<OptionMetadata> metadatas) {
            return new List<OptionMetadata>(metadatas)
                .FindAll(metadata => metadata.ArgumentType == ArgumentType.Named);
        }

        //----------------------------------------------------------------------[]
        public static IList<OptionMetadata> GetPositionalArguments(IList<OptionMetadata> metadatas) {
            return new List<OptionMetadata>(metadatas)
                .FindAll(delegate(OptionMetadata metadata) { return metadata.ArgumentType == ArgumentType.Positional; });
        }

        //----------------------------------------------------------------------[]
        public static OptionMetadata GetSingleUnboundOptionsMetadataOrThrow(IList<OptionMetadata> metadatas) {
            IList<OptionMetadata> result = new List<OptionMetadata>(metadatas)
                .FindAll(delegate(OptionMetadata metadata) { return metadata.ArgumentType == ArgumentType.Unbound; });
            if (result.Count == 0)
                return null;
            if (result.Count == 1) {
                return result[0];
            }
            throw new ArgumentException();
        }
        #endregion
    }
}