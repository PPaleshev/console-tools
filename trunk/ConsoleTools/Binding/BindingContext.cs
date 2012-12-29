using System;
using System.ComponentModel;
using ConsoleTools.Utils;


namespace ConsoleTools.Binding {
    internal class BindingContext : ITypeDescriptorContext {
        #region Data

        private readonly object _instance;
        private readonly PropertyDescriptor _descriptor;

        #endregion

        #region Construction

        public BindingContext(object instance, OptionMetadata metadata) {
            _instance = instance;
            _descriptor = metadata.PropertyDescriptor;
        }

        #endregion

        #region Implementation of IServiceProvider

        object IServiceProvider.GetService(Type serviceType) {
            throw new NotSupportedException();
        }

        #endregion

        #region Implementation of ITypeDescriptorContext

        bool ITypeDescriptorContext.OnComponentChanging() {
            throw new NotSupportedException();
        }

        //----------------------------------------------------------------------[]
        void ITypeDescriptorContext.OnComponentChanged() {
            throw new NotSupportedException();
        }

        //----------------------------------------------------------------------[]
        IContainer ITypeDescriptorContext.Container {
            get { throw new NotSupportedException(); }
        }

        //----------------------------------------------------------------------[]
        object ITypeDescriptorContext.Instance {
            get { return _instance; }
        }

        //----------------------------------------------------------------------[]
        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor {
            get { return _descriptor; }
        }

        #endregion
    }
}