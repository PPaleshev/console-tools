using System;
using System.ComponentModel;

namespace ConsoleTools.Binding {
    /// <summary>
    /// ���������� ��������� ����������.
    /// </summary>
    internal class BindingContext : ITypeDescriptorContext {
        /// <summary>
        /// ��������� �������, �������� ����������� ��������.
        /// </summary>
        private readonly object instance;

        /// <summary>
        /// ���������� �������� �������� �������.
        /// </summary>
        private readonly PropertyDescriptor descriptor;

        /// <summary>
        /// ������ ����� ��������� ��������� ����������.
        /// </summary>
        /// <param name="instance">��������� �������, �������� �������� �����������.</param>
        /// <param name="metadata">���������� �����, �������� ������� �����������.</param>
        public BindingContext(object instance, OptionMetadata metadata) {
            this.instance = instance;
            descriptor = metadata.PropertyDescriptor;
        }

        object IServiceProvider.GetService(Type serviceType) {
            throw new NotSupportedException();
        }

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
            get { return instance; }
        }

        //----------------------------------------------------------------------[]
        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor {
            get { return descriptor; }
        }
    }
}